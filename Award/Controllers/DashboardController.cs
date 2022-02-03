using Award.Web.Models;
using Microsoft.EntityFrameworkCore;
using Rotativa;
using Rotativa.AspNetCore;
using Award.Core.Interfaces;
using Award.Core.ViewModel;
using Award.Infrastructure.Services;
using Award.Web.Helper;
using Award.Web.Common.Auth;
using Award.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Award.Core.Entities;
using Microsoft.Extensions.Localization;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Award.Web.Controllers
{
    [Authorize(Roles = "Administrator , SuperAdmin")]
    public class DashboardController : BaseController
    {
        private readonly AwardDbContext _context;
        private IUserSession _userSession;
        private readonly IStringLocalizer<DashboardController> _localizer;
        private readonly ICategoryServices _categoryServices;

        public DashboardController(AwardDbContext context, IUserSession userSession, IStringLocalizer<DashboardController> localizer, ICategoryServices categoryServices) : base(context)
        {
            _context = context;
            _userSession = userSession;
            _localizer = localizer;
            _categoryServices = categoryServices;
        }
        public IActionResult Index()
        {
            return View(GetDashboardDetail());
        }
        public IActionResult AwardsList()
        {
            return View("AwardsList", GetAwardDetails());
        }
        public IActionResult SectorList()
        {
            return View("SectorList", GetSectors());
        }
        public IActionResult DepartmentList()
        {
            return View("DepartmentListNew", GetDepartments());
        }
        public IActionResult TeamList()
        {
            return View("TeamList", GetTeams());
        }
        public IActionResult CategoryList()
        {
            return View("CategoryList", GetCategoryList());
        }
        public IActionResult ParticipatedSectorList()
        {
            return View("ParticipatedSectorList", GetParticipatedSectorList());
        }
        public IActionResult IntrestedEmployees()
        {
            return View("IntrestedEmployeesList", GetIntrestedEmployeesDetail());
        }
        public IActionResult SubmissionFromQSM()
        {
            return View("SubmissionFromQSM", GetQSMSubmissionList());
        }
        public IActionResult SubmissionFromJury()
        {
            return View("SubmissionFromJury", GetJurySubmissionList());
        }
        public IActionResult AuditManagerSubmission()
        {
            return View("AuditManagerSubmission", GetAuditManagerSubmissionList());
        }
        public DashboardViewModel GetDashboardDetail()
        {
            var awards = _context.Awards.LongCountAsync().Result;
            var sector = _context.Sectors.LongCountAsync().Result;
            var Departments = _context.Departments.LongCountAsync().Result;
            var Categories = _context.Categories.LongCountAsync().Result;

            //IEnumerable<CategorySector> CategoriesSectors = _context.CategoriesSectors.ToList()
            //                                            .GroupBy(customer => customer.Sector.Id)
            //                                            .Select(group => group.First());

            var TotalSubmissionFromQsm = _context.QSMEntries.Count();
            var TotalSubmissionFromAuditManager = _context.CategoryTeamEntries.Count();
            var TotalSubmissionFromJury = _context.Winners.Count();
            var TotalTeams = _context.Teams.Count();
            var TotalParticipatedSectors = from x in _context.CategorySectorEntries
                                           group x by x.SectorId;

            var result = from y in TotalParticipatedSectors
                         select new
                         {
                             Id = y.Key,
                             Quantity = y.Count()
                         };
            var TotalParticipated = from x in _context.CategoryEntries
                                     group x by x.UserId;
            var resultParticipated = from y in TotalParticipated
                                     select new
                                     {
                                         Id = y.Key,
                                         Quantity = y.Count()
                                     };

            return new DashboardViewModel()
            {
                TotalAwards = awards,
                TotalCategories = Categories,
                TotalDepartments = Departments,
                TotalSectors = sector,
                TotalEmpShowIntrest = resultParticipated.Count(),
                TotalSubmissionFromQSM = TotalSubmissionFromQsm,
                TotalSubmissionFromAuditManager = TotalSubmissionFromAuditManager,
                TotalSubmissionFromJury = TotalSubmissionFromJury,
                TotalTeams = TotalTeams,
                ParticipatedSectors = result.Count()
            };
        }
        public List<AwardsViewModel> GetAwardDetails()
        {
            List<AwardsViewModel> list = new List<AwardsViewModel>();
            var awards = _context.Awards.ToList();
            foreach (var a in awards)
            {
                list.Add(new AwardsViewModel() { award = a });
            }
            return list;
        }
        public List<Sector> GetSectors()
        {
            return _context.Sectors.ToList();
        }
        public List<Department> GetDepartments()
        {
            return _context.Departments.ToList();
        }
        public List<Teams> GetTeams()
        {
            return _context.Teams.ToList();
        }
        public List<UsersDetail> GetIntrestedEmployeesDetail()
        {
            var query =
                    from e in _context.Employee
                    join u in _context.Users on e.Id equals u.IdEmployee 
                    join cE in _context.CategoryEntries on u.Id equals cE.UserId
                    join c in _context.Categories on cE.CategoryId equals c.Id
                    join a in _context.Awards on c.IdAward equals a.Id
                    select new UsersDetail()
                    {
                        EmployeeNameEn = e.NameEn,
                        AwardName = a.Name,
                        CatName = c.Name,
                        Gender = CurrentLanguage == "en" ? e.SexEn : e.SexAr,
                        Job = CurrentLanguage == "en" ? e.JobEn : e.JobAr
                    };

            return query.ToList();
        }
        public List<CategoryDashboardViewModel> GetCategoryList()
        {
            var query =
                   from Categories in _context.Categories
                   join Awards in _context.Awards on Categories.IdAward equals Awards.Id
                   select new CategoryDashboardViewModel()
                   {
                       CategoryName = Categories.Name,
                       CategoryId = Categories.Id,
                       AwardName = Awards.Name,
                       AwardId = Awards.Id,
                       AwardType = Categories.AwardType,
                       TotalExpressedIntrest = _context.CategoryEntries.Where(d => d.CategoryId == Categories.Id).Count()
                   };

            return query.ToList();
        }
        public List<ParticipatedSectorDashboardViewModel> GetParticipatedSectorList()
        {

            if (CurrentLanguage == "en")
            {
                var result = new List<ParticipatedSectorDashboardViewModel>();
                var tmp = from x in _context.CategorySectorEntries
                          join s in _context.Sectors on x.SectorId equals s.Id
                          group x by s.NameEn;
                var data = from y in tmp
                           select new
                           {
                               SectorName = y.Key,
                               CategoryCount = y.Count()
                           };
                foreach (var d in data)
                {
                    var t = d.SectorName;

                    result.Add(new ParticipatedSectorDashboardViewModel { CategoryCount = d.CategoryCount, SectorName = d.SectorName });
                }
                return result;

            }
            else
            {
                var result = new List<ParticipatedSectorDashboardViewModel>();
                var tmp = from x in _context.CategorySectorEntries
                          join s in _context.Sectors on x.SectorId equals s.Id
                          group x by s.NameAr;
                var data = from y in tmp
                           select new
                           {
                               SectorName = y.Key,
                               CategoryCount = y.Count()
                           };
                foreach (var d in data)
                {
                    var t = d.SectorName;

                    result.Add(new ParticipatedSectorDashboardViewModel { CategoryCount = d.CategoryCount, SectorName = d.SectorName });
                }
                return result;

            }
        }
        public List<QSMSubmissionViewModel> GetQSMSubmissionList()
        {
            var result = new List<QSMSubmissionViewModel>();

            var dt = _categoryServices.GetSubmissionFromQSM();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(new QSMSubmissionViewModel { AwardName = dt.Rows[i]["AwardName"].ToString(), CategoryName = dt.Rows[i]["CatName"].ToString(), AssignedUser = dt.Rows[i]["AssignedUser"].ToString(), QSM = dt.Rows[i]["SubmittedUser"].ToString(),SectorName= CurrentLanguage == "en" ? dt.Rows[i]["NameEn"].ToString() : dt.Rows[i]["NameAr"].ToString()});

            }
            return result;
        }
        public List<JurySubmissionViewModel> GetJurySubmissionList()
        {
            var result = new List<JurySubmissionViewModel>();

            var dt = _categoryServices.GetSubmissionFromJury();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(new JurySubmissionViewModel { AwardName = dt.Rows[i]["AwardName"].ToString(), CatName  = dt.Rows[i]["CatName"].ToString(), Winner  = dt.Rows[i]["Winner"].ToString(), Jury  = dt.Rows[i]["Jury"].ToString() });

            }
            return result;
        }
        public List<TeamMemberViewModel> GetAuditManagerSubmissionList()
        {
            var result = new List<TeamMemberViewModel>();
            var dt = _categoryServices.GetAuditManagerSubmission();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(new TeamMemberViewModel { CatName = dt.Rows[i]["CatName"].ToString(), Auditor = dt.Rows[i]["Auditor"].ToString(), AuditManager = dt.Rows[i]["AuditManager"].ToString(), SubmittedDate = Convert.ToDateTime(dt.Rows[i]["SubmittedDate"]) });
            }
            return result;
        }
    }
}