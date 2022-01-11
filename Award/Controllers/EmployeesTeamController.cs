using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Award.Core.Entities;
using Award.Core.Interfaces;
using Award.Infrastructure.Data;
using Award.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Award.Core.Common.Constants;

namespace Award.Web.Controllers
{
    [Authorize]

    public class EmployeesTeamController : BaseController
    {
        private readonly AwardDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public EmployeesTeamController(AwardDbContext context,
                        IConfiguration configuration,
            IUserService userService
            )
        {
            _context = context;
            this._configuration = configuration;
            this._userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await _context.Teams.ToListAsync().ConfigureAwait(false);
            var teamsVM = teams.Select(a => new TeamViewModel { TeamId = a.Id, TeamName = a.Name });

            return View(teamsVM);
        }

        public async Task<IActionResult> Create()
        {
            (List <User> allauditManagerList,List <User> allauditorList, List<User> allJuryList) employeeDetail = await SetViewBag();
            var model = new TeamViewModel();
            var auditManagerUserList = new List<TeamMemberViewModel>();
            var auditorUserList = new List<TeamMemberViewModel>();
            var juryUserList = new List<TeamMemberViewModel>();

            foreach (var data in employeeDetail.allauditManagerList)
            {
                auditManagerUserList.Add(new TeamMemberViewModel { UserName = data.FirstName, UserId = data.Id, IsSelected = false });
            }
            foreach (var data in employeeDetail.allauditorList)
            {
                auditorUserList.Add(new TeamMemberViewModel { UserName = data.FirstName, UserId = data.Id, IsSelected = false });
            }
            //foreach (var data in employeeDetail.allJuryList)
            //{
            //    juryUserList.Add(new TeamMemberViewModel { UserName = data.FirstName, UserId = data.Id, IsSelected = false });
            //}
            model.AuditManagerUsersList  = auditManagerUserList;
            model.AuditorUsersList = auditorUserList;
            //model.JuryUserList = juryUserList;
            return this.View(model);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamViewModel teamViewModel)
        {
            if (!string.IsNullOrWhiteSpace(teamViewModel.TeamName))
            {
                foreach (var data in teamViewModel.AuditManagerUsersList)
                {
                    if (data.IsSelected == true)
                        teamViewModel.AuditManagerUsersIdList.Add(data.UserId);
                }
                foreach (var data in teamViewModel.AuditorUsersList)
                {
                    if (data.IsSelected == true)
                        teamViewModel.AuditorUsersIdList.Add(data.UserId);
                }
                foreach (var data in teamViewModel.JuryUserList )
                {
                    if (data.IsSelected == true )
                        teamViewModel.JuryUserIdList.Add(data.UserId);
                }

                var allSelectedUsers = new List<TeamMemberViewModel>();
                allSelectedUsers.AddRange(teamViewModel.AuditManagerUsersIdList.Select(userId => new TeamMemberViewModel { UserId = userId,RoleId=4 }));
                allSelectedUsers.AddRange(teamViewModel.AuditorUsersIdList.Select(userId => new TeamMemberViewModel { UserId = userId,RoleId=5 }));
                allSelectedUsers.AddRange(teamViewModel.JuryUserIdList.Select(userId => new TeamMemberViewModel { UserId = userId, RoleId=6}));


                var team = new Teams { Id = teamViewModel.TeamId, Name = teamViewModel.TeamName, EmployeeTeams = allSelectedUsers.Select(a => new EmployeeTeams { UserId = a.UserId,RoleId=a.RoleId }).ToList() };

                await _userService.CreateTeam(team);

                return RedirectToAction(nameof(Index));
            }

            await SetViewBag();
            return this.View(teamViewModel);
        }

        public async Task<IActionResult> Edit(long teamId)
        {
            var allAuditManagerList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDIT_MANAGER);
            var allAuditorList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDITOR);
            var allJuryList = await _userService.GetAllUsersWithRoleNameAsync(Roles.JURY);


            var auditManagerUserList = new List<TeamMemberViewModel>();
            var auditorUserList = new List<TeamMemberViewModel>();
            var juryUserList = new List<TeamMemberViewModel>();

            var team = await _context.Teams.Include(a => a.EmployeeTeams).SingleAsync(a => a.Id == teamId).ConfigureAwait(false);


            var teamViewModel = new TeamViewModel
            {
                TeamId = team?.Id ?? 0,
                TeamName = team?.Name,
                AuditManagerUsersList = allAuditManagerList?.Select(a => new TeamMemberViewModel { User = a , IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id && b.RoleId==4) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
                AuditorUsersList = allAuditorList?.Select(a => new TeamMemberViewModel { User = a,IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id && b.RoleId == 5) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
                JuryUserList = allJuryList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id && b.RoleId == 6) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
            };

            teamViewModel.AuditManagerUsersIdList = teamViewModel.AuditManagerUsersList.Select(a => a.User.Id).ToList();
            teamViewModel.AuditorUsersIdList = teamViewModel.AuditorUsersList.Select(a => a.User.Id).ToList();
            teamViewModel.JuryUserIdList = teamViewModel.JuryUserList.Select(a => a.User.Id).ToList();

            foreach (var data in allAuditManagerList)
            { 
                auditManagerUserList.Add(new TeamMemberViewModel { UserName = data.FirstName, UserId = data.Id, IsSelected = teamViewModel.AuditManagerUsersIdList.Any(uId => uId == data.Id),RoleId=4 });
            }
            foreach (var data in allAuditorList)
            {
                auditorUserList.Add(new TeamMemberViewModel { UserName = data.FirstName, UserId = data.Id, IsSelected = teamViewModel.AuditorUsersIdList.Any(uId => uId == data.Id),RoleId=5 });
            }
            foreach (var data in allJuryList)
            {
                juryUserList.Add(new TeamMemberViewModel { UserName = data.FirstName, UserId = data.Id, IsSelected = teamViewModel.JuryUserIdList.Any(uId => uId == data.Id), RoleId = 6 });
            }
            teamViewModel.AuditManagerUsersList = auditManagerUserList;
            teamViewModel.AuditorUsersList = auditorUserList;
            teamViewModel.JuryUserList = juryUserList;

            return this.View(teamViewModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamViewModel teamViewModel)
        {
            if (teamViewModel.TeamId > 0 && !string.IsNullOrWhiteSpace(teamViewModel.TeamName))
            {
                var employeeTeams = new List<EmployeeTeams>();
                foreach (var data in teamViewModel.AuditManagerUsersList.Where(a => a.IsSelected == true))
                { 
                    employeeTeams.Add(new EmployeeTeams { TeamId = teamViewModel.TeamId, UserId = data.UserId, RoleId = 4 });
                }
                foreach (var data in teamViewModel.AuditorUsersList.Where(a => a.IsSelected == true))
                {
                    employeeTeams.Add(new EmployeeTeams { TeamId = teamViewModel.TeamId, UserId = data.UserId, RoleId = 5 });
                }
                foreach (var data in teamViewModel.JuryUserList.Where(a => a.IsSelected == true))
                {
                    employeeTeams.Add(new EmployeeTeams { TeamId = teamViewModel.TeamId, UserId = data.UserId, RoleId = 6 });
                }

                var team = new Teams { Id = teamViewModel.TeamId, Name = teamViewModel.TeamName, EmployeeTeams = employeeTeams };
                await _userService.EditEmployeesTeam(team);
                return RedirectToAction(nameof(Index));
            }

            await SetViewBag(teamViewModel);
            return this.View(teamViewModel);
        }

        public async Task<IActionResult> Details(long teamId)
        {
            var allAuditManagerList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDIT_MANAGER);
            var allAuditorList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDITOR);
            var allJuryList = await _userService.GetAllUsersWithRoleNameAsync(Roles.JURY);

            var team = await _context.Teams.Include(a => a.EmployeeTeams).SingleAsync(a => a.Id == teamId).ConfigureAwait(false);

            //var teamViewModel = new TeamViewModel
            //{
            //    TeamId = team?.Id ?? 0,
            //    TeamName = team?.Name,
            //    AuditManagerUsersList = allAuditManagerList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
            //    AuditorUsersList = allAuditorList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
            //    JuryUserList = allJuryList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id) == true ? true : false })?.Where(a => a.IsSelected)?.ToList()
            //};


            var teamViewModel = new TeamViewModel
            {
                TeamId = team?.Id ?? 0,
                TeamName = team?.Name,
                AuditManagerUsersList = allAuditManagerList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id && b.RoleId == 4) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
                AuditorUsersList = allAuditorList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id && b.RoleId == 5) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
                JuryUserList = allJuryList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id && b.RoleId == 6) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
            };



            return this.View(teamViewModel);
        }

        public async Task<IActionResult> Delete(long teamId)
        {
            var allAuditManagerList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDIT_MANAGER);
            var allAuditorList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDITOR);
            var allJuryList = await _userService.GetAllUsersWithRoleNameAsync(Roles.JURY);

            var team = await _context.Teams.Include(a => a.EmployeeTeams).SingleAsync(a => a.Id == teamId).ConfigureAwait(false);
            var teamViewModel = new TeamViewModel
            {
                TeamId = team?.Id ?? 0,
                TeamName = team?.Name,
                AuditManagerUsersList = allAuditManagerList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id && b.RoleId == 4) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
                AuditorUsersList = allAuditorList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id && b.RoleId == 5) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
                JuryUserList = allJuryList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.EmployeeTeams?.Any(b => b.UserId == a.Id && b.RoleId == 6) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
            };


            return this.View(teamViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TeamViewModel teamViewModel)
        {
            await _userService.DeleteTeam(teamViewModel.TeamId);
            return RedirectToAction(nameof(Index));
        }
        private async Task<(List<User> auditManagerList,List<User> auditorList, List<User> juryList)> SetViewBag()
        {
            var teamViewModel = new List<User>();
            List<SelectListItem> items = new List<SelectListItem>();
            var allAuditManagerList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDIT_MANAGER);
            var allAuditorList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDITOR);
            var allJuryList = await _userService.GetAllUsersWithRoleNameAsync(Roles.JURY);

            //allAuditManagerList = allAuditManagerList.Where(a => !_context.EmployeeTeams.Where(e => e.RoleId == 4).Select(e => e.UserId).Contains(a.Id)).ToList();
            //allAuditorList = allAuditorList.Where(a => !_context.EmployeeTeams.Where(e=>e.RoleId==5).Select(e => e.UserId).Contains(a.Id)).ToList();
            //allJuryList = allJuryList.Where(a => !_context.EmployeeTeams.Select(e => e.UserId).Contains(a.Id)).ToList();

            ViewBag.AuditManagerUsersList = allAuditManagerList.Select(a => new SelectListItem { Value = a.Id.ToString(), Text =CurrentLanguage=="en"? a.FirstName:a.LastName }).ToList();
            ViewBag.AuditorUsersList = allAuditorList.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = CurrentLanguage == "en" ? a.FirstName : a.LastName }).ToList();
            ViewBag.JuryUserList = allJuryList.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = CurrentLanguage == "en" ? a.FirstName : a.LastName }).ToList();
            return (auditManagerList: allAuditManagerList.ToList(),auditorList: allAuditorList.ToList(), juryList: allJuryList.ToList());

        }
        private async Task SetViewBag(TeamViewModel teamViewModel)
        {
            var allAuditManagerList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDIT_MANAGER);
            var allAuditorList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDITOR);
            var allJuryList = await _userService.GetAllUsersWithRoleNameAsync(Roles.JURY);

            ViewBag.AuditManagerUsersList = allAuditManagerList.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FirstName, Selected = teamViewModel.AuditManagerUsersIdList.Any(uId => uId == a.Id) }).ToList();
            ViewBag.AuditorUsersList = allAuditorList.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FirstName, Selected = teamViewModel.AuditorUsersIdList.Any(uId => uId == a.Id) }).ToList();
            ViewBag.JuryUserList = allJuryList.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FirstName, Selected = teamViewModel.JuryUserIdList.Any(uId => uId == a.Id) }).ToList();
        }
    }
}