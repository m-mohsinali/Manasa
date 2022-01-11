using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Award.Core.Common;
using Award.Core.Entities;
using Award.Infrastructure.Data;
using Award.Web.Common.Utils;
using Award.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace Award.Web.Controllers
{
    [Authorize]

    public class EvaluationController : BaseController
    {
        private readonly AwardDbContext _context;
        private readonly IFileInfo _fileInfo;
        [Obsolete]
        private readonly IHostingEnvironment _hostEnv;
        private readonly IStringLocalizer<NotificationController> _localizer;
        Years _year = new Years();

        [Obsolete]
        public EvaluationController(AwardDbContext context, IFileInfo fileInfo, IHostingEnvironment hostEnv, IStringLocalizer<NotificationController> localizer) : base(context)
        {
            _context = context;
            _fileInfo = fileInfo;
            _hostEnv = hostEnv;
            _localizer = localizer;
        }
        public IActionResult Index(int saveStatus = 0, string role = "")
        {
            ViewData["AwardYearSelectList"] = new SelectList(_year.Year(), "Text", "Value", "");

            var teamMemberViewModel = new List<TeamMemberViewModel>();
            if (IsRolePresent("Administrator"))
            {

                var result = (from q in _context.QSMEntries
                              join ct in _context.CategoryTeamEntries on q.Id equals ct.QsmEntryId
                              join u in _context.Users on q.AssignedUserId equals u.Id
                              join te in _context.TeamEntry on ct.Id equals te.CatigoryTeamEntryId
                              join c in _context.Categories on q.CategoryId equals c.Id
                              join a in _context.Awards on c.IdAward equals a.Id
                              join cd in _context.CategoryDocuments on c.Id equals cd.Category.Id
                              into mma
                              from cd in mma.DefaultIfEmpty()
                              select new
                              {
                                  UserId = u.Id,
                                  UserName = CurrentLanguage == "en" ? u.FirstName : u.FirstName,
                                  Status = "Completed",//roleId == 4 ? (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.AuditmanageruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted") : (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.CurrentStatusId == 2 && c.AuditoruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted"),
                                  CategoryTeamsId = ct.Id,
                                  CatName = c.Name,
                                  AwardName = a.Name,
                                  AuditorId = te.AuditoruserId,
                                  QsmEntryId = q.Id,
                                  CatId = c.Id,
                                  TeamId = ct.TeamId


                              }

                            ).ToList();

                foreach (var data in result)
                {
                    teamMemberViewModel.Add(new TeamMemberViewModel
                    {
                        UserId = data.UserId,
                        UserName = data.UserName,
                        Status = data.Status,
                        CategoryTeamsId = data.CategoryTeamsId,
                        CatName = data.CatName,
                        Award = data.AwardName,
                        AuditorId = data.AuditorId,
                        CatId = data.CatId,
                        TeamId = data.TeamId
                    });
                }
                var resultlamba = teamMemberViewModel.GroupBy(stu => stu.CatId).OrderBy(stu => stu.Key);
                var teamMemberViewModelGroupedData = new List<TeamMemberViewModel>();
                foreach (var group in resultlamba)
                {
                    int i = 1;
                    foreach (var res in group)
                    {
                        if (i == 1)
                            teamMemberViewModelGroupedData.Add(new TeamMemberViewModel { CatName = res.CatName, Award = res.Award, CatId = res.CatId, TeamId = res.TeamId });
                        i++;
                    }
                }
                teamMemberViewModel = null;
                teamMemberViewModel = teamMemberViewModelGroupedData;
            }
            else
            {
                long roleId = 0;
                //var teamIds = (from et in _context.EmployeeTeams
                //               where et.UserId == long.Parse(CurrentUserId)
                //               select new
                //               {
                //                   teamid = et.TeamId
                //               }
                //             ).ToList();
                if (IsRolePresent("AuditManager"))
                {
                    roleId = 4;
                    var teamIds = (from et in _context.EmployeeTeams
                                   where et.UserId == long.Parse(CurrentUserId) && et.RoleId == 4
                                   select new
                                   {
                                       teamid = et.TeamId
                                   }
                            ).ToList();

                    foreach (var id in teamIds)
                    {

                        var result = (from q in _context.QSMEntries
                                      join ct in _context.CategoryTeamEntries on q.Id equals ct.QsmEntryId
                                      join u in _context.Users on q.AssignedUserId equals u.Id
                                      join c in _context.Categories on q.CategoryId equals c.Id
                                      join a in _context.Awards on c.IdAward equals a.Id

                                      where ct.TeamId == id.teamid
                                      select new
                                      {
                                          UserId = u.Id,
                                          UserName = CurrentLanguage == "en" ? u.FirstName : u.FirstName,
                                          Status = roleId == 4 ? (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.AuditmanageruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted") : (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.CurrentStatusId == 2 && c.AuditoruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted"),
                                          CategoryTeamsId = ct.Id,
                                          CatName = c.Name,
                                          Award = a.Name,
                                          CatId = c.Id
                                      }
                                    ).ToList();

                        foreach (var data in result)
                        {
                            teamMemberViewModel.Add(new TeamMemberViewModel { UserId = data.UserId, UserName = data.UserName, Status = data.Status, CategoryTeamsId = data.CategoryTeamsId, CatName = data.CatName, Award = data.Award, CatId = data.CatId });
                        }
                    }
                }
                if (IsRolePresent("Auditor"))
                {
                    roleId = 5;
                    var teamIds = (from et in _context.EmployeeTeams
                                   where et.UserId == long.Parse(CurrentUserId) && et.RoleId == 5
                                   select new
                                   {
                                       teamid = et.TeamId
                                   }
                           ).ToList();
                    foreach (var id in teamIds)
                    {

                        var result = (from q in _context.QSMEntries
                                      join ct in _context.CategoryTeamEntries on q.Id equals ct.QsmEntryId
                                      join u in _context.Users on q.AssignedUserId equals u.Id
                                      join te in _context.TeamEntry on ct.Id equals te.CatigoryTeamEntryId
                                      join c in _context.Categories on q.CategoryId equals c.Id
                                      join a in _context.Awards on c.IdAward equals a.Id
                                      //join cd in _context.CategoryDocuments on c.Id equals cd.Category.Id
                                      //into mma
                                      //from cd in mma.DefaultIfEmpty()
                                      where ct.TeamId == id.teamid && te.AuditoruserId == long.Parse(CurrentUserId)
                                      select new
                                      {
                                          UserId = u.Id,
                                          UserName = CurrentLanguage == "en" ? u.FirstName : u.FirstName,
                                          Status = roleId == 4 ? (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.AuditmanageruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted") : (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.CurrentStatusId == 2 && c.AuditoruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted"),
                                          CategoryTeamsId = ct.Id,
                                          CatName = c.Name,
                                          AwardName = a.Name,
                                          CatId = c.Id

                                      }

                                    ).ToList();

                        foreach (var data in result)
                        {
                            teamMemberViewModel.Add(new TeamMemberViewModel { UserId = data.UserId, UserName = data.UserName, Status = data.Status, CategoryTeamsId = data.CategoryTeamsId, CatName = data.CatName, Award = data.AwardName, CatId = data.CatId });
                        }
                    }
                }

                if (IsRolePresent("Jury"))
                {
                    roleId = 6;


                    var isJuryMember = _context.JuryTeam.Any(j => j.EmployeeId == long.Parse(CurrentUserId));
                    if (isJuryMember)
                    {
                        var auditorIdList = new List<long>();
                        var teamIds = (from et in _context.EmployeeTeams
                                           // where et.UserId == long.Parse(CurrentUserId) //&& et.RoleId == 6
                                       select new
                                       {
                                           teamid = et.TeamId
                                       }
                               ).ToList();


                        //var teamId = _context.EmployeeTeams.Where(e => e.UserId == long.Parse(CurrentUserId))?.FirstOrDefault().TeamId;

                        var empTeam = (from et in _context.EmployeeTeams
                                       join ur in _context.UserRoles on et.UserId equals ur.UserId
                                       where  ur.RoleId == 5
                                       select new
                                       {
                                           AuditorId = et.UserId
                                       }
                                     ).ToList();
                        for (int i = 0; i < empTeam.Count; i++)
                        {
                            auditorIdList.Add(empTeam[i].AuditorId);
                        }

                        foreach (var id in teamIds)
                        {
                            var result = (from q in _context.QSMEntries
                                          join ct in _context.CategoryTeamEntries on q.Id equals ct.QsmEntryId
                                          join te in _context.TeamEntry on ct.Id equals te.CatigoryTeamEntryId
                                          join c in _context.Categories on q.CategoryId equals c.Id
                                          join a in _context.Awards on c.IdAward equals a.Id
                                          where ct.TeamId == id.teamid && auditorIdList.Contains(te.AuditoruserId)//te.AuditoruserId == long.Parse(CurrentUserId)
                                                                                                                  //orderby 1
                                          select new
                                          {
                                              // UserId = u.Id,
                                              // UserName = CurrentLanguage == "en" ? u.FirstName : u.FirstName,
                                              Status = roleId == 4 ? (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.AuditmanageruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted") : (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.CurrentStatusId == 2 && c.AuditoruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted"),
                                              CategoryTeamsId = ct.Id,
                                              CatName = c.Name,
                                              AwardName = a.Name,
                                              AuditorId = te.AuditoruserId,
                                              QsmEntryId = q.Id,
                                              CatId = c.Id

                                          }

                                        ).ToList();



                            foreach (var data in result)
                            {
                                teamMemberViewModel.Add(new TeamMemberViewModel { Status = data.Status, CatName = data.CatName, Award = data.AwardName, CatId = data.CatId });
                            }
                        }

                        var resultlamba = teamMemberViewModel.GroupBy(stu => stu.CatId).OrderBy(stu => stu.Key);
                        var teamMemberViewModelGroupedData = new List<TeamMemberViewModel>();
                        foreach (var group in resultlamba)
                        {
                            int i = 1;
                            foreach (var res in group)
                            {
                                if (i == 1)
                                    teamMemberViewModelGroupedData.Add(new TeamMemberViewModel { CatName = res.CatName, Award = res.Award, CatId = res.CatId });
                                i++;
                            }
                        }
                        teamMemberViewModel = null;
                        teamMemberViewModel = teamMemberViewModelGroupedData;
                    }
                    else
                    {

                        //teamMemberViewModel = null;
                        //teamMemberViewModel = new List<TeamMemberViewModel>();

                    }

                }
            }
            ViewBag.saveStatus = saveStatus;
            return View("~/Views/Evaluation/Index.cshtml", teamMemberViewModel);
        }

        public IActionResult AllocateTeamLead(long userId, long catTeamId, string cat, string Award)
        {
            try
            {

                var isExist = _context.TeamEntry.FirstOrDefault(c => c.AuditmanageruserId == long.Parse(CurrentUserId) && c.CatigoryTeamEntryId == catTeamId);

                if (isExist == null)
                {
                    TeamEntry teamEntry = new TeamEntry();
                    teamEntry.CurrentStatusId = 1;
                    teamEntry.AuditmanageruserId = long.Parse(CurrentUserId);
                    teamEntry.AuditoruserId = userId;
                    teamEntry.CatigoryTeamEntryId = catTeamId;
                    teamEntry.CreateDate = DateTime.Now;
                    _context.AddAsync(teamEntry).ConfigureAwait(false);
                    string notificationMessage = _localizer["SubmissionFromAuditManager"];
                    notificationMessage = notificationMessage.Replace("AuditManagerUser", CurrentUserFullName);
                    notificationMessage = notificationMessage.Replace("AwardName", Award);
                    notificationMessage = notificationMessage.Replace("CatName", cat);
                    InsertNotification(userId, notificationMessage, 5);

                }
                else
                {
                    _context.TeamEntry.Remove(isExist);
                    string notificationMessage = _localizer["UnAllocationFromAuditManager"];
                    notificationMessage = notificationMessage.Replace("AuditManagerUser", CurrentUserFullName);
                    notificationMessage = notificationMessage.Replace("AwardName", Award);
                    notificationMessage = notificationMessage.Replace("CatName", cat);
                    InsertNotification(userId, notificationMessage, 5);

                }
                _context.SaveChangesAsync();

                return Json(new { result = "Sucessfully" });
            }
            catch (Exception ex)
            {
                return Json("");

            }
        }

        public IActionResult ShowTeamLead(long userId, long catTeamId)
        {
            try
            {

                var result = (from et in _context.EmployeeTeams
                              join ct in _context.CategoryTeamEntries on et.TeamId equals ct.TeamId
                              join u in _context.Users on et.UserId equals u.Id
                              //join ur in _context.UserRoles on u.Id equals ur.UserId
                              where ct.Id == catTeamId && et.RoleId == 5
                              select new
                              {
                                  UserId = u.Id,
                                  UserName = u.FirstName,
                                  //Status = roleId == 4 ? (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.AuditmanageruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted") : (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.CurrentStatusId == 2 && c.AuditoruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted"),
                                  CategoryTeamsId = ct.Id,
                                  isSubmitted = _context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.AuditmanageruserId == long.Parse(CurrentUserId) && c.AuditoruserId == u.Id).Id.ToString() == null ? false : true
                              }
                            ).ToList();
                return Json(new { result = result });
            }
            catch (Exception ex)
            {
                return Json("");

            }
        }

        [Obsolete]
        public FileResult Download()
        {
            List<string> files = new List<string>() { @"Document\Category\a34f3ce6-1689-4ab4-8ebd-381f172106b0.jpg" };
            var archive = _hostEnv.WebRootPath;
            // Server.MapPath("~/archive.zip");
            var temp = _hostEnv.WebRootPath + "/temp";

            // clear any existing archive
            if (System.IO.File.Exists(archive))
            {
                System.IO.File.Delete(archive);
            }
            // empty the temp folder
            Directory.EnumerateFiles(temp).ToList().ForEach(f => System.IO.File.Delete(f));

            // copy the selected files to the temp folder
            files.ForEach(f => System.IO.File.Copy(_hostEnv.WebRootPath + "\\" + f, Path.Combine(temp, Path.GetFileName(_hostEnv.WebRootPath + "\\" + f))));

            // create a new archive
            ZipFile.CreateFromDirectory(temp, archive);

            return File(archive, "application/zip", "archive.zip");
        }

        public IActionResult ViewAllocatedCategory(long userId, long catTeamId, long auditorId = 0)
        {
            try
            {
                if (IsRolePresent("Auditor"))
                { auditorId = long.Parse(CurrentUserId); }
                var auditingUserAnswerViewModel = new List<AuditingUserAnswerViewModel>();

                if (IsRolePresent("AuditManager"))
                {
                    var auditManagerId = long.Parse(CurrentUserId);
                    var result = (from qsm in _context.QSMEntries
                                  join cte in _context.CategoryTeamEntries on qsm.Id equals cte.QsmEntryId
                                  join u in _context.Users on qsm.AssignedUserId equals u.Id
                                  join c in _context.Categories on qsm.CategoryId equals c.Id
                                  join cc in _context.CategoryCriterias on c.Id equals cc.CategoryId
                                  join csc in _context.CategorySubCriterias on cc.Id equals csc.CategoryCriteriaId
                                  join ua in _context.UserAnswers on csc.Id equals ua.SubCriteriaId

                                  join aua in _context.AuditingUserAnswer on ua.Id equals aua.UserAnwserId
                                  into AuditUserAnswer
                                  from aua in AuditUserAnswer.DefaultIfEmpty()
                                  where cte.Id == catTeamId && qsm.Id == ua.QsmEntryId
                                  select new
                                  {
                                      criteria = cc.Description,
                                      subCiteria = csc.Description,
                                      maxMarks = csc.Marks,
                                      Answer = ua.Answer,
                                      Marks = aua.Marks,
                                      Strength = aua.Strength,
                                      Suggestion = aua.Suggestion,
                                      SubCriteriaId = csc.Id,
                                      // TeamEntryId = te.Id,
                                      Document1 = _context.CriteriaDocuments.Where(x => x.SubCriteriaId == csc.Id && x.QSMEntryId == qsm.Id && x.DocumentNo == 1).FirstOrDefault().SupportingDocumentPath == null ? "" : _context.CriteriaDocuments.Where(x => x.SubCriteriaId == csc.Id && x.QSMEntryId == qsm.Id && x.DocumentNo == 1).FirstOrDefault().SupportingDocumentPath,
                                      Document2 = _context.CriteriaDocuments.Where(x => x.SubCriteriaId == csc.Id && x.QSMEntryId == qsm.Id && x.DocumentNo == 2).FirstOrDefault().SupportingDocumentPath == null ? "" : _context.CriteriaDocuments.Where(x => x.SubCriteriaId == csc.Id && x.QSMEntryId == qsm.Id && x.DocumentNo == 2).FirstOrDefault().SupportingDocumentPath,
                                      CatId = c.Id,
                                      QsmEntryId = qsm.Id,
                                      UserAnwserId = ua.Id

                                  }
                  ).ToList();

                    foreach (var data in result)
                    {

                        // var auditingAnswerDetail = _context.AuditingUserAnswer.Where(a => a.TeamEntryId == data.TeamEntryId && a.SubCriteriaId == data.SubCriteriaId).FirstOrDefault();

                        auditingUserAnswerViewModel.Add(new AuditingUserAnswerViewModel
                        {
                            Description = data.subCiteria,
                            MaxMarks = data.maxMarks,
                            Answer = data.Answer,
                            Marks = 0,
                            Strength = "Accessor will decide",
                            Suggestion = "Accessor will decide",
                            SubCriteriaId = data.SubCriteriaId,
                            // TeamEntryId = data.TeamEntryId,
                            Document1 = data.Document1,
                            Document2 = data.Document2,
                            CatId = data.CatId,
                            QsmEntryId = data.QsmEntryId,
                            Criteria = data.criteria,
                            UserAnswerId = data.UserAnwserId

                        });
                    }
                }

                else
                {
                    var result = (from qsm in _context.QSMEntries
                                  join cte in _context.CategoryTeamEntries on qsm.Id equals cte.QsmEntryId
                                  join u in _context.Users on qsm.AssignedUserId equals u.Id
                                  join te in _context.TeamEntry on cte.Id equals te.CatigoryTeamEntryId
                                  join c in _context.Categories on qsm.CategoryId equals c.Id
                                  join cc in _context.CategoryCriterias on c.Id equals cc.CategoryId
                                  join csc in _context.CategorySubCriterias on cc.Id equals csc.CategoryCriteriaId
                                  join ua in _context.UserAnswers on csc.Id equals ua.SubCriteriaId
                                  join aua in _context.AuditingUserAnswer on ua.Id equals aua.UserAnwserId
                                  into AuditUserAnswer
                                  from aua in AuditUserAnswer.DefaultIfEmpty()
                                  where cte.Id == catTeamId && te.AuditoruserId == auditorId && qsm.Id == ua.QsmEntryId
                                  select new
                                  {
                                      criteria = cc.Description,
                                      subCiteria = csc.Description,
                                      maxMarks = csc.Marks,
                                      Answer = ua.Answer,
                                      Marks = aua.Marks.ToString() == null ? 0 : aua.Marks,
                                      Strength = aua.Strength == null ? "" : aua.Strength,
                                      Suggestion = aua.Suggestion == null ? "" : aua.Suggestion,
                                      SubCriteriaId = csc.Id,
                                      TeamEntryId = te.Id,
                                      Document1 = _context.CriteriaDocuments.Where(x => x.SubCriteriaId == csc.Id && x.QSMEntryId == qsm.Id && x.DocumentNo == 1).FirstOrDefault().SupportingDocumentPath == null ? "" : _context.CriteriaDocuments.Where(x => x.SubCriteriaId == csc.Id && x.QSMEntryId == qsm.Id && x.DocumentNo == 1).FirstOrDefault().SupportingDocumentPath,
                                      Document2 = _context.CriteriaDocuments.Where(x => x.SubCriteriaId == csc.Id && x.QSMEntryId == qsm.Id && x.DocumentNo == 2).FirstOrDefault().SupportingDocumentPath == null ? "" : _context.CriteriaDocuments.Where(x => x.SubCriteriaId == csc.Id && x.QSMEntryId == qsm.Id && x.DocumentNo == 2).FirstOrDefault().SupportingDocumentPath,
                                      CatId = c.Id,
                                      QsmEntryId = qsm.Id,
                                      UserAnwserId = ua.Id

                                  }
                   ).ToList();

                    foreach (var data in result)
                    {

                        var auditingAnswerDetail = _context.AuditingUserAnswer.Where(a => a.TeamEntryId == data.TeamEntryId && a.SubCriteriaId == data.SubCriteriaId).FirstOrDefault();

                        auditingUserAnswerViewModel.Add(new AuditingUserAnswerViewModel
                        {
                            Description = data.subCiteria,
                            MaxMarks = data.maxMarks,
                            Answer = data.Answer,
                            Marks = auditingAnswerDetail == null ? data.Marks : auditingAnswerDetail.Marks,
                            Strength = auditingAnswerDetail == null ? data.Strength : auditingAnswerDetail.Strength,
                            Suggestion = auditingAnswerDetail == null ? data.Suggestion : auditingAnswerDetail.Suggestion,
                            SubCriteriaId = data.SubCriteriaId,
                            TeamEntryId = data.TeamEntryId,
                            Document1 = data.Document1,
                            Document2 = data.Document2,
                            CatId = data.CatId,
                            QsmEntryId = data.QsmEntryId,
                            Criteria = data.criteria,
                            UserAnswerId = data.UserAnwserId

                        });
                    }
                }



                return View("~/Views/Evaluation/SaveEvaluation.cshtml", auditingUserAnswerViewModel);


            }
            catch (Exception ex)
            {
                return Json("");

            }
        }

        public IActionResult ViewTopScorer(long catId, long teamIdNew = 0)
        {
            try
            {
                ViewData["AwardYearSelectList"] = new SelectList(_year.Year(), "Text", "Value", "");

                var teamMemberViewModel = new List<TeamMemberViewModel>();
                long roleId = 0;

                if (IsRolePresent("Jury"))
                {
                    //var teamIds = (from et in _context.EmployeeTeams
                    //               where et.UserId == long.Parse(CurrentUserId)
                    //               select new
                    //               {
                    //                   teamid = et.TeamId
                    //               }
                    //             ).ToList();
                    roleId = 6;
                    var auditorIdList = new List<long>();

                    //var teamId = _context.EmployeeTeams.Where(e => e.UserId == long.Parse(CurrentUserId))?.FirstOrDefault().TeamId;

                    var empTeam = (from et in _context.EmployeeTeams
                                   join ur in _context.UserRoles on et.UserId equals ur.UserId
                                   where   ur.RoleId == 5
                                   select new
                                   {
                                       AuditorId = et.UserId
                                   }
                                 ).ToList();
                    for (int i = 0; i < empTeam.Count; i++)
                    {
                        auditorIdList.Add(empTeam[i].AuditorId);
                    }

                   // var groupedTeamIds = teamIds.GroupBy(t => t.teamid).ToList();


                    //foreach (var id in groupedTeamIds)
                    //{
                        var result = (from q in _context.QSMEntries
                                      join ct in _context.CategoryTeamEntries on q.Id equals ct.QsmEntryId
                                      join u in _context.Users on q.AssignedUserId equals u.Id
                                      join te in _context.TeamEntry on ct.Id equals te.CatigoryTeamEntryId
                                      join c in _context.Categories on q.CategoryId equals c.Id
                                      join a in _context.Awards on c.IdAward equals a.Id
                                      where auditorIdList.Contains(te.AuditoruserId) && c.Id == catId//te.AuditoruserId == long.Parse(CurrentUserId)
                                                                                                                               //orderby 1
                                      select new
                                      {
                                          UserId = u.Id,
                                          UserName = CurrentLanguage == "en" ? u.FirstName : u.FirstName,
                                          Status = roleId == 4 ? (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.AuditmanageruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted") : (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.CurrentStatusId == 2 && c.AuditoruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted"),
                                          CategoryTeamsId = ct.Id,
                                          CatName = c.Name,
                                          AwardName = a.Name,
                                          AuditorId = te.AuditoruserId,
                                          QsmEntryId = q.Id,
                                          CatId = c.Id

                                      }

                                    ).ToList();



                        foreach (var data in result)
                        {
                            long totalMarks = (from ua in _context.UserAnswers
                                               join aua in _context.AuditingUserAnswer on ua.Id equals aua.UserAnwserId
                                               where ua.QsmEntryId == data.QsmEntryId
                                               select aua.Marks).Sum();
                            teamMemberViewModel.Add(new TeamMemberViewModel { UserId = data.UserId, UserName = data.UserName, Status = data.Status, CategoryTeamsId = data.CategoryTeamsId, CatName = data.CatName, Award = data.AwardName, AuditorId = data.AuditorId, TotalMarks = totalMarks, CatId = data.CatId });
                            //teamMemberViewModel.Add(new TeamMemberViewModel { Status = data.Status, CatName = data.CatName, Award = data.AwardName, CatId = data.CatId });

                        }
                        teamMemberViewModel = teamMemberViewModel.OrderByDescending(x => x.TotalMarks).ToList();
                    //}
                }
                if (IsRolePresent("Administrator"))
                {
                    roleId = 1;
                    var result = (from q in _context.QSMEntries
                                  join ct in _context.CategoryTeamEntries on q.Id equals ct.QsmEntryId
                                  join u in _context.Users on q.AssignedUserId equals u.Id
                                  join te in _context.TeamEntry on ct.Id equals te.CatigoryTeamEntryId
                                  join c in _context.Categories on q.CategoryId equals c.Id
                                  join a in _context.Awards on c.IdAward equals a.Id
                                  where c.Id == catId
                                  //where ct.TeamId == id.teamid && auditorIdList.Contains(te.AuditoruserId) && c.Id == catId//te.AuditoruserId == long.Parse(CurrentUserId)
                                  //orderby 1
                                  select new
                                  {
                                      UserId = u.Id,
                                      UserName = CurrentLanguage == "en" ? u.FirstName : u.FirstName,
                                      Status = roleId == 4 ? (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.AuditmanageruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted") : (_context.TeamEntry.FirstOrDefault(c => c.CatigoryTeamEntryId == ct.Id && c.CurrentStatusId == 2 && c.AuditoruserId == long.Parse(CurrentUserId)).Id.ToString() == null ? "Review" : "Submitted"),
                                      CategoryTeamsId = ct.Id,
                                      CatName = c.Name,
                                      AwardName = a.Name,
                                      AuditorId = te.AuditoruserId,
                                      QsmEntryId = q.Id,
                                      CatId = c.Id,
                                      AwardId = a.Id,
                                      AlreadyAnnounced = _context.WinnerAnnouncements.Where(x => x.UserId == u.Id && x.CategoryId == c.Id && x.AwardId == a.Id).Count()

                                  }
             ).ToList();

                    foreach (var data in result)
                    {
                        long totalMarks = (from ua in _context.UserAnswers
                                           join aua in _context.AuditingUserAnswer on ua.Id equals aua.UserAnwserId
                                           where ua.QsmEntryId == data.QsmEntryId
                                           select aua.Marks).Sum();
                        teamMemberViewModel.Add(new TeamMemberViewModel { UserId = data.UserId, UserName = data.UserName, Status = data.Status, CategoryTeamsId = data.CategoryTeamsId, CatName = data.CatName, Award = data.AwardName, AuditorId = data.AuditorId, TotalMarks = totalMarks, CatId = data.CatId, AwardId = data.AwardId, AlreadyAnnounced = data.AlreadyAnnounced });
                    }
                    teamMemberViewModel = teamMemberViewModel.OrderByDescending(x => x.TotalMarks).ToList();
                }
                return View("~/Views/Evaluation/TopScorer.cshtml", teamMemberViewModel);
            }
            catch (Exception ex)
            {
                return Json("");

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAuditingUserAnswer(List<AuditingUserAnswerViewModel> model)
        {
            try
            {
                foreach (var data in model)
                {
                    AuditingUserAnswers auditingUserAnswer = new AuditingUserAnswers();
                    TeamEntry teamEntry = new TeamEntry();
                    var isSave = 0;

                    var recordExists = (
                    from aua in _context.AuditingUserAnswer
                    where aua.TeamEntryId == data.TeamEntryId && aua.SubCriteriaId == data.SubCriteriaId
                    select new { Id = aua.Id }
                    ).ToList();
                    if (recordExists?.Count() > 0)
                    {
                        auditingUserAnswer = (from a in _context.AuditingUserAnswer
                                              where a.Id == recordExists[0].Id
                                              select a).SingleOrDefault();
                    }
                    else
                    {
                        isSave = 1;
                    }
                    auditingUserAnswer.SubCriteriaId = data.SubCriteriaId;
                    auditingUserAnswer.Marks = data.Marks;
                    auditingUserAnswer.Strength = data.Strength;
                    auditingUserAnswer.Suggestion = data.Suggestion;
                    auditingUserAnswer.TeamEntryId = data.TeamEntryId;
                    auditingUserAnswer.CurrentStatusId = 1;
                    auditingUserAnswer.UserAnwserId = data.UserAnswerId;

                    teamEntry = (from te in _context.TeamEntry
                                 where te.Id == data.TeamEntryId
                                 select te).SingleOrDefault();
                    teamEntry.CurrentStatusId = 2;
                    var catDetail = (from c in _context.Categories
                                     join a in _context.Awards on c.IdAward equals a.Id
                                     where c.Id == model[0].CatId
                                     select new
                                     {
                                         catName = c.Name,
                                         AwardName = a.Name
                                     }).FirstOrDefault();

                    if (isSave == 1)
                    {
                        await _context.AddAsync(auditingUserAnswer).ConfigureAwait(false);


                        if (User.IsInRole("Auditor"))
                        {
                            var currentTeamId = (from u in _context.EmployeeTeams
                                                 where u.UserId == long.Parse(CurrentUserId)
                                                 select new
                                                 {
                                                     u.TeamId
                                                 }

                                            ).FirstOrDefault();

                            var juryDetail = (from u in _context.EmployeeTeams
                                              join ur in _context.UserRoles on u.UserId equals ur.UserId
                                              where ur.RoleId == 6 && u.TeamId == currentTeamId.TeamId
                                              select new
                                              {
                                                  u.UserId
                                              }

                                            ).ToList();

                            foreach (var ids in juryDetail)
                            {
                                string notificationMessage = _localizer["NotificationMessageForJury"];
                                notificationMessage = notificationMessage.Replace("CatName", catDetail.catName);
                                notificationMessage = notificationMessage.Replace("AwardName", catDetail.AwardName);
                                notificationMessage = notificationMessage.Replace("AuditorName", CurrentUserFullName);
                                InsertNotification(ids.UserId, notificationMessage, 5);
                            }

                        }

                        if (User.IsInRole("Jury"))
                        {
                            var adminList = _context.UserRoles.Where(ur => ur.RoleId == 1).ToList();

                            foreach (var ids in adminList)
                            {
                                string notificationMessage = _localizer["NotificationMessageForAdminFromJury"];
                                notificationMessage = notificationMessage.Replace("CatName", catDetail.catName);
                                notificationMessage = notificationMessage.Replace("AwardName", catDetail.AwardName);
                                notificationMessage = notificationMessage.Replace("JuryName", CurrentUserFullName);
                                InsertNotification(ids.UserId, notificationMessage, 7);
                            }

                        }



                    }
                    else
                    {
                        if (User.IsInRole("Auditor"))
                        {
                            var currentTeamId = (from u in _context.EmployeeTeams
                                                 where u.UserId == long.Parse(CurrentUserId)
                                                 select new
                                                 {
                                                     u.TeamId
                                                 }

                                            ).FirstOrDefault();

                            var juryDetail = (from u in _context.EmployeeTeams
                                              join ur in _context.UserRoles on u.UserId equals ur.UserId
                                              where ur.RoleId == 6 && u.TeamId == currentTeamId.TeamId
                                              select new
                                              {
                                                  u.UserId
                                              }

                                            ).ToList();

                            foreach (var ids in juryDetail)
                            {
                                string notificationMessage = _localizer["InformationModified"];
                                notificationMessage = notificationMessage.Replace("CatName", catDetail.catName);
                                notificationMessage = notificationMessage.Replace("AwardName", catDetail.AwardName);
                                notificationMessage = notificationMessage.Replace("UserName", CurrentUserFullName);
                                InsertNotification(ids.UserId, notificationMessage, 5);
                            }

                        }

                        if (User.IsInRole("Jury"))
                        {
                            var adminList = _context.UserRoles.Where(ur => ur.RoleId == 1).ToList();

                            foreach (var ids in adminList)
                            {
                                string notificationMessage = _localizer["InformationModified"];
                                notificationMessage = notificationMessage.Replace("CatName", catDetail.catName);
                                notificationMessage = notificationMessage.Replace("AwardName", catDetail.AwardName);
                                notificationMessage = notificationMessage.Replace("UserName", CurrentUserFullName);
                                InsertNotification(ids.UserId, notificationMessage, 7);
                            }

                        }
                    }
                }
                await _context.SaveChangesAsync();


                return RedirectToAction("Index", "Evaluation", new { saveStatus = 1 });  //ViewAllocatedCategory(long.Parse( CurrentUserId), 1,1);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Evaluation", new { saveStatus = 2 });  //ViewAllocatedCategory(long.Parse( CurrentUserId), 1,1);


                // return await FillCriteria(model.Category.Id, "2");
                /// throw;
            }
        }
        [HttpGet]
        public ActionResult LoadTeamsData(long qsmId)
        {
            var categoryTeamEntries = _context.CategoryTeamEntries.Where(cse => cse.QsmEntryId == qsmId).ToList();
            var team = (from s in _context.Teams
                        join et in _context.EmployeeTeams on s.Id equals et.TeamId
                        join u in _context.Users on et.UserId equals u.Id
                        //join ur in _context.UserRoles on u.Id equals ur.UserId
                        where et.RoleId == 4
                        select new
                        {
                            Id = s.Id,
                            Name = s.Name,
                            IsSubmitted = false,
                            UserName = CurrentLanguage == "en" ? u.FirstName : u.LastName
                        }).ToList();

            List<CategoryTeamEntries> result = new List<CategoryTeamEntries>();

            for (int i = 0; i < team.Count; i++)
            {
                var isSubmitted = categoryTeamEntries.Any(cse => cse.TeamId == team[i].Id);
                result.Add(new CategoryTeamEntries { Id = team[i].Id, Name = team[i].Name, IsSubmitted = isSubmitted, UserName = team[i].UserName });
            }
            return Json(new { result = result });
        }
        [HttpGet]
        public ActionResult AllocateTeam(long qsmId, long teamId, long catId)
        {
            var isExist = _context.CategoryTeamEntries.FirstOrDefault(c => c.TeamId == teamId && c.QsmEntryId == qsmId);
            var auditManagerId = (from et in _context.EmployeeTeams
                                  join ur in _context.UserRoles on et.UserId equals ur.UserId
                                  where ur.RoleId == 4 && et.TeamId == teamId
                                  select new { et.UserId }).FirstOrDefault();
            var catDetail = (from c in _context.Categories
                             join a in _context.Awards on c.IdAward equals a.Id
                             where c.Id == catId
                             select new
                             {
                                 catName = c.Name,
                                 awardName = a.Name
                             }).FirstOrDefault();


            if (isExist == null)
            {
                var categoryTeams = new CategoryTeamEntry();
                categoryTeams.CreateDate = DateTime.Now;
                categoryTeams.TeamId = teamId;
                categoryTeams.QsmEntryId = qsmId;
                categoryTeams.CurrentStatusId = 1;
                _context.Add(categoryTeams);
                _context.SaveChanges();
                var t = categoryTeams.Id;
                string notificationMessage = _localizer["NotificationMessageForAutidManager"];
                notificationMessage = notificationMessage.Replace("AdminUser", CurrentUserFullName);
                notificationMessage = notificationMessage.Replace("AwardName", catDetail.awardName);
                notificationMessage = notificationMessage.Replace("CatName", catDetail.catName);

                InsertNotification(auditManagerId.UserId, notificationMessage, 4);

            }
            else
            {

                _context.CategoryTeamEntries.Remove(isExist);
                string notificationMessage = _localizer["CancelNotificationMessageForAutidManager"];
                notificationMessage = notificationMessage.Replace("AdminUser", CurrentUserFullName);
                notificationMessage = notificationMessage.Replace("AwardName", catDetail.awardName);
                notificationMessage = notificationMessage.Replace("CatName", catDetail.catName);
                InsertNotification(auditManagerId.UserId, notificationMessage, 4);

            }
            _context.SaveChangesAsync();

            return Json(new { result = "Sucessfully" });
        }
    }
}