using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Award.Infrastructure.Data;
using Award.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Award.Web.Controllers
{
    [Authorize]

    public class JuryOperationsController : BaseController
    {
        private readonly AwardDbContext _context;
        public JuryOperationsController(AwardDbContext context) : base(context)
        { 
            _context = context;

        }
        public IActionResult Index(int saveStatus = 0)
        {
            List<JuryOperationsViewModel> juryOperationsViewModel = new List<JuryOperationsViewModel>();

            var result = (from aua in _context.AuditingUserAnswer
                          join te in _context.TeamEntry on aua.TeamEntryId equals te.Id
                          join cte in _context.CategoryTeamEntries on te.CatigoryTeamEntryId equals cte.Id
                          join qe in _context.QSMEntries on cte.QsmEntryId equals qe.Id
                          join c in _context.Categories on qe.CategoryId equals c.Id
                          join a in _context.Awards on c.IdAward equals a.Id
                          join u in _context.Users on qe.AssignedUserId equals u.Id
                          join e in _context.Employee on u.IdEmployee equals e.Id
                          join s in _context.Sectors on e.SectorId equals s.Id
                          join et in _context.EmployeeTeams on cte.TeamId equals et.TeamId
                          join ur in _context.UserRoles on et.UserId equals ur.UserId

                          where te.CurrentStatusId == 2 && ur.UserId == long.Parse(CurrentUserId) && ur.RoleId == 6
                          select new
                          {
                              a = aua.Id,
                              AwardName = a.Name,
                              CatName = c.Name,
                              EmpName = CurrentLanguage == "en" ? e.NameEn : e.NameAr,
                              Sector = CurrentLanguage == "en" ? s.NameEn : s.NameAr,
                              Score = aua.Marks,
                              CatId = c.Id,
                              AwardId = a.Id,
                              SectorId = s.Id,
                              EmpId = e.Id,
                              IsAnnounced = false
                              // EvaluatedBy = _context.Users.Where(u => u.Id == et.UserId && u.Id == _context.UserRoles.Where(ur=>ur.RoleId ==5).FirstOrDefault().UserId).FirstOrDefault().FirstName
                          }
                           ).ToList();


            foreach (var data in result)
            {

                var awardDetail = _context.Winners.Where(w => w.SectorId == data.SectorId && w.AwardId == data.AwardId && w.CategoryId == data.CatId && w.EmployeeId == data.EmpId).FirstOrDefault();
                juryOperationsViewModel.Add(new JuryOperationsViewModel
                {
                    AwardTittle = data.AwardName,
                    CategoryName = data.CatName,
                    ParticipatedBy = data.EmpName,
                    Sector = data.Sector,
                    ScoredMarks = data.Score,
                    Id = data.a,
                    SectorId = data.SectorId,
                    CatId = data.CatId,
                    AnounceDate = awardDetail == null ? "" : awardDetail.AnounceDate.ToString(),
                    AwardId = data.AwardId,
                    EmpId = data.EmpId,
                    IsAnnounced = awardDetail == null ? false : true
                });
            }
            ViewBag.saveStatus = saveStatus;
            return View(juryOperationsViewModel);
        }
    }
}