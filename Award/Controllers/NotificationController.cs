using System.Threading.Tasks;
using Award.Core.Common;
using Award.Infrastructure.Data;
using Award.Models;
using Award.Core.Entities;
using Award.Web.Common.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Award.Web.Common.Auth;
using Microsoft.AspNetCore.Authorization;
using Award.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Award.Web.Controllers
{
    [Authorize]

    public class NotificationController : BaseController
    {
        private readonly AwardDbContext _context;
        private readonly IFileInfo _fileInfo;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly ILdapAuthenticationService _ldapAuthenticationService;
        public NotificationController(AwardDbContext context, IFileInfo fileInfo, IWebHostEnvironment hostEnv, ILdapAuthenticationService ldapAuthenticationService) : base(context)
        {
            _context = context;
            _fileInfo = fileInfo;
            _hostEnv = hostEnv;
            _ldapAuthenticationService = ldapAuthenticationService;
        }

        public IActionResult Index()
        {
            List<NotificationViewModel> result = GetUserNotifications(long.Parse(CurrentUserId));
            return View(result);
        }

        public IActionResult NotificationDetail(long NotificationId)
        {
            NotificationViewModel result = GetUserNotificationById(long.Parse(CurrentUserId), NotificationId);
            return View(result);
        }

        public IActionResult Calendar()
        {
            return View();
        }

        public JsonResult GetNotificationList()
        {
            List<NotificationViewModel> result = GetUserNotifications(long.Parse(CurrentUserId));
            return Json(result);
        }
     

        public JsonResult UnReadNotification()
        {
           var count = _context.Notifications.Where(d => d.UserId == long.Parse(CurrentUserId) && d.CheckDate == null).Count();
            return Json(count);
        }
        public JsonResult GetUserEventList()
        {
            var events = _context.UserEvents.Where(d => d.FromUserId == long.Parse(CurrentUserId) || d.ToUserId == long.Parse(CurrentUserId))
                .Select( d=> new  { name = d.EventDescription, date = d.StartDate.ToString("MMMM dd, yyyy"), type="event" });
            return Json(events);
        }

        private List<NotificationViewModel> GetUserNotifications(long UserId)
        {
            List<NotificationViewModel> notification = new List<NotificationViewModel>();

            var result = _context.Notifications.Where(d => d.UserId == UserId).OrderByDescending(d => d.CreateDate).Select(not => new NotificationViewModel
            {
                NotificationId = not.Id,
                NotificationText = not.NotificationText.Length > 20 ? string.Concat(not.NotificationText.Substring(0, 20), "...") : not.NotificationText,
                NotificationType = CurrentLanguage == "en" ? not.NotificationType.TypeEn : not.NotificationType.TypeAr,
                NotificationTypeId = not.NotificationType.Id,
                IsRead = not.CheckDate == null ? false : true,
                Created = not.CreateDate
            }).ToList();
            return result;
        }

        private NotificationViewModel GetUserNotificationById(long UserId,long NotificationId)
        {
            var model = _context.Notifications.Where(d => d.UserId == UserId && d.Id == NotificationId).FirstOrDefault();

            model.CheckDate = DateTime.Now;
            _context.Update(model);
            _context.SaveChanges();

            var result = _context.Notifications.Where(d => d.UserId == UserId && d.Id == NotificationId).Select(not => new NotificationViewModel
            {
                NotificationId = not.Id,
                NotificationText = not.NotificationText,
                NotificationType = CurrentLanguage == "en" ? not.NotificationType.TypeEn : not.NotificationType.TypeAr,
                NotificationTypeId = not.NotificationType.Id,
                IsRead = not.CheckDate == null ? false : true,
                Created = not.CreateDate
            }).FirstOrDefault();
            return result;
        }
    }
}