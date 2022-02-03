using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Award.Core.Entities;
using Award.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Award.Core.Interfaces;
using Award.Web.Models;
using static Award.Core.Common.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Award.Web.Controllers
{
    [Authorize(Roles = "Administrator , SuperAdmin")]
    public class JuryTeamsController : BaseController
    {
        private readonly AwardDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public JuryTeamsController(AwardDbContext context, IConfiguration configuration, IUserService userService)
        {
            this._context = context;
            this._configuration = configuration;
            this._userService = userService;
        }

        // GET: JuryTeams
        public async Task<IActionResult> Index()
        {
            var ids = _context.JuryTeam.Select(a => a.EmployeeId.ToString()).ToArray();
            var employeeDetail = await SetViewBag(ids);
            var model = new TeamViewModel();
            var juryUserList = new List<TeamMemberViewModel>();

            foreach (var data in employeeDetail)
            {
                juryUserList.Add(new TeamMemberViewModel { UserName = data.FirstName, UserId = data.Id, IsSelected = true });
            }
            model.JuryUserList = juryUserList;
            return this.View(model);
        }

        // GET: JuryTeams/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            var myList = new List<string>();
            myList.Add(id.ToString());
            var employeeDetail = await SetViewBag(myList.ToArray());
            var model = new TeamViewModel();
            var juryUserList = new List<TeamMemberViewModel>();

            foreach (var data in employeeDetail)
            {
                juryUserList.Add(new TeamMemberViewModel { UserName = data.FirstName, UserId = data.Id, IsSelected = true });
            }
            model.JuryUserList = juryUserList;
            return this.View(model);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var juryTeam = await _context.JuryTeam
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (juryTeam == null)
            //{
            //    return NotFound();
            //}

            //return View(juryTeam);
        }

        // GET: JuryTeams/Create
        public async Task<IActionResult> Create()
        {
            List<User> employeeDetail = await SetViewBag(null);
            var model = new TeamViewModel();
            var juryUserList = new List<TeamMemberViewModel>();

            foreach (var data in employeeDetail)
            {
                juryUserList.Add(new TeamMemberViewModel { UserName = data.FirstName, UserId = data.Id, IsSelected = _context.JuryTeam.Where(a=>a.EmployeeId ==data.Id).ToList().Count()>0?true:false });
            }

            model.JuryUserList = juryUserList;
            return this.View(model);
        }

        // POST: JuryTeams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create(TeamViewModel teamViewModel)
        {
            //var juryTeam = new JuryTeam();



            foreach (var data in teamViewModel.JuryUserList)
            {
                if (data.IsSelected == true)
                    teamViewModel.JuryUserIdList.Add(data.UserId);
            }

            var allSelectedUsers = new List<TeamMemberViewModel>();
            allSelectedUsers.AddRange(teamViewModel.JuryUserIdList.Select(userId => new TeamMemberViewModel { UserId = userId, RoleId = 6 }));


            var juryTeam = new List<JuryTeam>();

            foreach (var data in allSelectedUsers)
            {
                juryTeam.Add(new JuryTeam { EmployeeId = data.UserId, Status = 1, CreateDate = DateTime.Now });
            }
            await _userService.CreateJuryTeam(juryTeam);

            //return View(teamViewModel);
            return RedirectToAction("Index");

        }

        // GET: JuryTeams/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {

            var juryId = new List<string>();
            juryId.Add(id.ToString());
            List<User> employeeDetail = await SetViewBag(null);
            var model = new TeamViewModel();
            var juryUserList = new List<TeamMemberViewModel>();

            foreach (var data in employeeDetail)
            {
                juryUserList.Add(new TeamMemberViewModel { UserName = data.FirstName, UserId = data.Id, IsSelected = true });
            }

            model.JuryUserList = juryUserList;
            return this.View(model);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var juryTeam = await _context.JuryTeam.FindAsync(id);
            //if (juryTeam == null)
            //{
            //    return NotFound();
            //}
            //return View(juryTeam);
        }

        // POST: JuryTeams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("EmployeeId,Status,Id,CreateDate,UpdatedDate,DeletedDate")] JuryTeam juryTeam)
        {
            if (id != juryTeam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(juryTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JuryTeamExists(juryTeam.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(juryTeam);
        }

        // GET: JuryTeams/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var juryTeam = await _context.JuryTeam
                .FirstOrDefaultAsync(m => m.Id == id);
            if (juryTeam == null)
            {
                return NotFound();
            }

            return View(juryTeam);
        }

        // POST: JuryTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var juryTeam = await _context.JuryTeam.FindAsync(id);
            _context.JuryTeam.Remove(juryTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JuryTeamExists(long id)
        {
            return _context.JuryTeam.Any(e => e.Id == id);
        }
        private async Task<List<User>> SetViewBag(string[] ids)
        {
            var teamViewModel = new List<User>();
            List<SelectListItem> items = new List<SelectListItem>();
            // var allAuditManagerList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDIT_MANAGER);
            //  var allAuditorList = await _userService.GetAllUsersWithRoleNameAsync(Roles.AUDITOR);
            //var allJuryList = await _userService.GetAllUsersWithRoleNameAsync(Roles.JURY);
            var allJuryList = await _userService.GetAllUsersWithRoleNameAsyncByIds(Roles.JURY, ids);

            //allAuditManagerList = allAuditManagerList.Where(a => !_context.EmployeeTeams.Where(e => e.RoleId == 4).Select(e => e.UserId).Contains(a.Id)).ToList();
            //allAuditorList = allAuditorList.Where(a => !_context.EmployeeTeams.Where(e=>e.RoleId==5).Select(e => e.UserId).Contains(a.Id)).ToList();
            //allJuryList = allJuryList.Where(a => !_context.EmployeeTeams.Select(e => e.UserId).Contains(a.Id)).ToList();

            //  ViewBag.AuditManagerUsersList = allAuditManagerList.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = CurrentLanguage == "en" ? a.FirstName : a.LastName }).ToList();
            //  ViewBag.AuditorUsersList = allAuditorList.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = CurrentLanguage == "en" ? a.FirstName : a.LastName }).ToList();
            ViewBag.JuryUserList = allJuryList.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = CurrentLanguage == "en" ? a.FirstName : a.LastName }).ToList();
            return allJuryList.ToList();

        }
    }
}
