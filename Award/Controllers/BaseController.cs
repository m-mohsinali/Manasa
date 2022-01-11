using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Award.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Award.Core.Entities;
using Award.Web.Models;
using Microsoft.AspNetCore.Http;

namespace Award.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly AwardDbContext _context;
        public BaseController(AwardDbContext context)
        {
            _context = context;
        }
        public BaseController()
        {
        }
        public string CurrentLanguage => System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        public string CurrentUserId => userid();
        public string CurrentUserName => username();
        public string CurrentUserFullName => usserFullName();
        public bool Isadmin => IsRolePresent("Administrator");
        public bool IsQSM => IsRolePresent("QualitySectionManager");
        public bool IsEndUser => IsRolePresent("EndUser");
        public bool IsAuditManager => IsRolePresent("AuditManager");
        public bool IsAuditor => IsRolePresent("Auditor");
        public bool IsJury => IsRolePresent("Jury");
        public string CurrentUserPic => UserPhoto();
        public bool IsRolePresent(string role)
        {
            return _context.LastLogInUser.Where(l => l.RoleName == role && l.UserId == long.Parse(CurrentUserId)).ToList().Count > 0 ? true : false;
        }
        private string userid()
        {
            return User.FindFirst(ClaimTypes.Sid)?.Value?.ToString() == null ? "0" : User.FindFirst(ClaimTypes.Sid)?.Value?.ToString();
        }
        private string username()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value?.ToString();
        }
        private string usserFullName()
        {
            return _context.Users.Where(u => u.Id == long.Parse(CurrentUserId)).FirstOrDefault().FirstName;

        }
        public string UserPhoto()
        {

            var currentUserDetail = (from ur in _context.UserRoles
                                     join u in _context.Users on ur.UserId equals u.Id
                                     join e in _context.Employee on u.IdEmployee equals e.Id
                                     join r in _context.Roles on ur.RoleId equals r.Id
                                     where u.Id == long.Parse(CurrentUserId)
                                     select new
                                     {
                                         e.EmployeePhotoUrl,
                                         r.Name

                                     }).ToList();
            string userRole = "";
            string empProfilePic = "";

            foreach (var data in currentUserDetail)
            {
                userRole = userRole == "" ? data.Name : userRole + "," + data.Name;
                empProfilePic = data.EmployeePhotoUrl;
            }

            return empProfilePic;
        }
        private string UserRoles()
        {

            var currentUserDetail = (from ur in _context.UserRoles
                                     join u in _context.Users on ur.UserId equals u.Id
                                     join e in _context.Employee on u.IdEmployee equals e.Id
                                     join r in _context.Roles on ur.RoleId equals r.Id
                                     where u.Id == long.Parse(CurrentUserId)
                                     select new
                                     {
                                         e.EmployeePhotoUrl,
                                         r.Name

                                     }).ToList();
            string userRole = "";
            string empProfilePic = "";

            foreach (var data in currentUserDetail)
            {
                userRole = userRole == "" ? data.Name : userRole + "," + data.Name;
                empProfilePic = data.EmployeePhotoUrl;
            }

            return userRole;

        }
        public List<UserRoles> UserRolesList(int roleId)
        {
            HttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
            var userRoles = new List<UserRoles>();

            var totalRoles = _context.UserRoles.Where(r => r.UserId == long.Parse(CurrentUserId)).ToList();
            if (roleId == 0)
            {
                if (totalRoles != null && totalRoles.Count == 1)
                {
                    var currentUserDetail = (from ur in _context.UserRoles
                                             join u in _context.Users on ur.UserId equals u.Id
                                             join e in _context.Employee on u.IdEmployee equals e.Id
                                             join r in _context.Roles on ur.RoleId equals r.Id
                                             where u.Id == long.Parse(CurrentUserId)
                                             select new
                                             {
                                                 e.EmployeePhotoUrl,
                                                 r.Name,
                                                 r.Id,
                                                 empName = CurrentLanguage == "en" ? e.NameEn : e.NameEn

                                             }).ToList();
                    foreach (var data in currentUserDetail)
                    {
                        userRoles.Add(new UserRoles { RoleName = data.Name, EmpImage = data.EmployeePhotoUrl, RoleId = data.Id, EmployeeName = data.empName });
                    }
                }

            }
            else
            {
                var currentUserDetail = (from ur in _context.UserRoles
                                         join u in _context.Users on ur.UserId equals u.Id
                                         join e in _context.Employee on u.IdEmployee equals e.Id
                                         join r in _context.Roles on ur.RoleId equals r.Id
                                         where u.Id == long.Parse(CurrentUserId) && r.Id == roleId
                                         select new
                                         {
                                             e.EmployeePhotoUrl,
                                             r.Name,
                                             r.Id,
                                             empName = CurrentLanguage == "en" ? e.NameEn : e.NameEn

                                         }).ToList();
                foreach (var data in currentUserDetail)
                {
                    userRoles.Add(new UserRoles { RoleName = data.Name, EmpImage = data.EmployeePhotoUrl, RoleId = data.Id, EmployeeName = data.empName });
                }
            }


            return userRoles;
        }
        public void InsertNotification(long ForUserId, string NotificationText, long NotificationTypeId)
        {
            var notification = new Notification()
            {
                UserId = ForUserId,
                NotificationText = NotificationText,
                NotificationTypeId = NotificationTypeId,
                CreateDate = DateTime.Now

            };
            _context.Add(notification);
            _context.SaveChanges();
        }
        public string InsertUserEvents(long FromUserId, long ToUserId, string eventDescription, DateTime start, DateTime end)
        {
            var from = _context.UserEvents.Where(d => d.FromUserId == FromUserId).ToList();
            foreach (var v in from)
            {
                bool overlap = start < v.EndDate && v.StartDate < end;
                if (overlap)
                { return "From User busy"; }

            }

            var to = _context.UserEvents.Where(d => d.ToUserId == ToUserId).ToList();
            foreach (var v in to)
            {
                bool overlap = start < v.EndDate && v.StartDate < end;
                if (overlap)
                { return "From User busy"; }
            }

            var userevent = new UserEvent()
            {
                FromUserId = FromUserId,
                ToUserId = ToUserId,
                EventDescription = eventDescription,
                StartDate = start,
                EndDate = end,
                CreateDate = DateTime.Now
            };
            _context.Add(userevent);
            _context.SaveChanges();

            return "";
        }
    }
}