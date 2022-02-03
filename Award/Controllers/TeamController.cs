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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Award.Core.Common.Constants;

namespace Award.Web.Controllers
{
    [Authorize(Roles = "Administrator , SuperAdmin")]

    public class TeamController : Controller
    {
        private readonly AwardDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public TeamController(AwardDbContext context,
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
            var categories = await _context.Categories.ToListAsync().ConfigureAwait(false);
            categories.ForEach(c => c.Award = _context.Awards.SingleOrDefault(a => a.Id == c.IdAward));
            return View(categories);
        }

        public async Task<IActionResult> Edit(long categoryId)
        {
            var allAuditManagerList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDIT_MANAGER);
            var allAuditorList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDITOR);
            var allJuryList = await _userService.GetAllUsersWithRoleNameAsync(Roles.JURY);

            var team = await _userService.GetCategoryTeam(_userService.GetCategoryTeamFromCategoryId(categoryId)?.Id??0);

            var teamViewModel = new TeamViewModel
            {
                TeamId = team?.Id ?? 0,
                //CategoryId = categoryId,
                //AuditManagerUsersList = allAuditManagerList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.CategoryTeams?.Any(b => b.UserId == a.Id) == true ? true : false }).ToList(),
                //AuditorUsersList = allAuditorList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.CategoryTeams?.Any(b => b.UserId == a.Id) == true ? true : false }).ToList(),
                //JuryUserList = allJuryList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.CategoryTeams?.Any(b => b.UserId == a.Id) == true ? true : false }).ToList()
            };

            return this.View(teamViewModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamViewModel teamViewModel)
        {
            var allSelectedUsers = new List<TeamMemberViewModel>();
            allSelectedUsers.AddRange(teamViewModel.AuditManagerUsersList.Where(a => a.IsSelected));
            allSelectedUsers.AddRange(teamViewModel.AuditorUsersList.Where(a => a.IsSelected));
            allSelectedUsers.AddRange(teamViewModel.JuryUserList.Where(a => a.IsSelected));
            var team = new Teams { Id = teamViewModel.TeamId, CategoryTeams = allSelectedUsers.Select(a => new CategoryTeamEntry { TeamId = Convert.ToInt32(teamViewModel.TeamId), Id  = teamViewModel.CategoryId}).ToList() };//Need to Check
            await _userService.CreateOrEditCategoryTeam(team);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(long categoryId)
        {
            var allAuditManagerList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDIT_MANAGER);
            var allAuditorList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDITOR);
            var allJuryList = await _userService.GetAllUsersWithRoleNameAsync(Roles.JURY);

            var team = await _userService.GetCategoryTeam(_userService.GetCategoryTeamFromCategoryId(categoryId)?.Id ?? 0);

            var teamViewModel = new TeamViewModel
            {
                TeamId = team?.Id ?? 0,
                CategoryId = categoryId,
                //AuditManagerUsersList = allAuditManagerList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.CategoryTeams?.Any(b => b.UserId == a.Id) == true ? true : false })?.Where(a=>a.IsSelected)?.ToList(),
                //AuditorUsersList = allAuditorList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.CategoryTeams?.Any(b => b.UserId == a.Id) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
                //JuryUserList = allJuryList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.CategoryTeams?.Any(b => b.UserId == a.Id) == true ? true : false })?.Where(a => a.IsSelected)?.ToList()
            };

            return this.View(teamViewModel);
        }

        public async Task<IActionResult> Delete(long categoryId)
        {
            var allAuditManagerList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDIT_MANAGER);
            var allAuditorList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDITOR);
            var allJuryList = await _userService.GetAllUsersWithRoleNameAsync(Roles.JURY);

            var team = await _userService.GetCategoryTeam(_userService.GetCategoryTeamFromCategoryId(categoryId)?.Id ?? 0);

            var teamViewModel = new TeamViewModel
            {
                TeamId = team?.Id ?? 0,
                CategoryId = categoryId,
                //AuditManagerUsersList = allAuditManagerList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.CategoryTeams?.Any(b => b.UserId == a.Id) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
                //AuditorUsersList = allAuditorList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.CategoryTeams?.Any(b => b.UserId == a.Id) == true ? true : false })?.Where(a => a.IsSelected)?.ToList(),
                //JuryUserList = allJuryList?.Select(a => new TeamMemberViewModel { User = a, IsSelected = team?.CategoryTeams?.Any(b => b.UserId == a.Id) == true ? true : false })?.Where(a => a.IsSelected)?.ToList()
            };

            return this.View(teamViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TeamViewModel teamViewModel)
        {
            var team = new Teams { Id = teamViewModel.TeamId};
            await _userService.DeleteCategoryTeam(team);
            return RedirectToAction(nameof(Index));
        }
    }
}