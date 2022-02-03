using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.AspNetCore.Http;

namespace Award.Web.Controllers
{
    [Authorize(Roles = "Administrator , SuperAdmin")]
    public class AnnouncementController : BaseController
    {
        private readonly AwardDbContext _context;
        private readonly IFileInfo _fileInfo;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly ILdapAuthenticationService _ldapAuthenticationService;
        public AnnouncementController(AwardDbContext context, IFileInfo fileInfo, IWebHostEnvironment hostEnv, ILdapAuthenticationService ldapAuthenticationService) : base(context)
        {
            _context = context;
            _fileInfo = fileInfo;
            _hostEnv = hostEnv;
            _ldapAuthenticationService = ldapAuthenticationService;
        }



        // GET: Announcements
        public async Task<IActionResult> Index()
        {
            //var isValid = _ldapAuthenticationService.ValidateUser("newton", "password");
            //var user = _ldapAuthenticationService.GetUser("newton", "password");
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            return View(await _context.Announcements.ToListAsync());
        }

        // GET: Announcements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // GET: Announcements/Create
        public async Task<IActionResult> Create()
        {
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Description,StartAnnouncingDate,EndAnnouncingDate,CreatedAt,DeletedAt")] Announcement announcement)
        public async Task<IActionResult> Create(AnnouncementViewModel announcementVm)
        {
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            if (!ModelState.IsValid)
            {
                return View(announcementVm);
            }

            announcementVm.Announcement.ThumbnailImagePath = await _fileInfo
                .SaveUploadFile(_hostEnv.WebRootPath,
                    AppSetting.Instance.AnnouncementImagePath,
                                announcementVm.ThumbnailImage).ConfigureAwait(false);

            announcementVm.Announcement.Name = System.IO.Path.GetFileName(announcementVm.ThumbnailImage.FileName);
            announcementVm.Announcement.CreatedAt = DateTime.Now;
            _context.Add(announcementVm.Announcement);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction("Index");
        }

        // GET: Announcements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements.FindAsync(id);

            var announcementViewModel = new AnnouncementViewModel();
            announcementViewModel.Announcement = announcement;

            if (announcement == null)
            {
                return NotFound();
            }
            return View(announcementViewModel);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Announcement.Id,Announcement.Title,Announcement.Description,Announcement.StartAnnouncingDate,Announcement.EndAnnouncingDate,Announcement.CreatedAt,DeletedAt,Name,Announcement.ThumbnailImagePath")] AnnouncementViewModel announcement)
        public async Task<IActionResult> Edit(long id, AnnouncementViewModel announcementViewModel)

        {
            if (id != announcementViewModel.Announcement.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(announcementViewModel);
            }
            try
            {

                announcementViewModel.Announcement.ThumbnailImagePath = await _fileInfo
                    .SaveUploadFile(_hostEnv.WebRootPath,
                        AppSetting.Instance.AnnouncementImagePath,
                                    announcementViewModel.ThumbnailImage).ConfigureAwait(false);

                _context.Update(announcementViewModel.Announcement);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementExists(announcementViewModel.Announcement.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            // return RedirectToAction(nameof(Index), "Index");
            return RedirectToAction(nameof(Index));
        }

        // GET: Announcements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            var announcement = await _context.Announcements.FindAsync(id);
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncementExists(long id)
        {

            return _context.Announcements.Any(e => e.Id == id);
        }

        //private Employee GetEmployeeInfoFromAD(string userName, string password, string staffUsername = "")
        //{
        //    var employeeProfile = new Employee();
        //    var directoryEntry = new DirectoryEntry(Settings.ActiveDirectoryPath, Settings.ActiveDirectoryUserName, Settings.ActiveDirectoryPassword);
        //    var searcher = new DirectorySearcher(directoryEntry);
        //    if (!string.IsNullOrEmpty(staffUsername))
        //    {
        //        searcher.Filter = "(&(objectClass=user)(anr=" + staffUsername + "))";
        //    }
        //    else
        //    {
        //        searcher.Filter = "(&(objectClass=user)(anr=" + userName + "))";
        //    }
        //    searcher.PropertiesToLoad.Add("mail");
        //    searcher.PropertiesToLoad.Add("displayName");
        //    searcher.PropertiesToLoad.Add("givenName");
        //    searcher.PropertiesToLoad.Add("department");
        //    searcher.PropertiesToLoad.Add("ipPhone");
        //    searcher.PropertiesToLoad.Add("password");
        //    if (searcher.FindOne() != null)
        //    {
        //        var prop = searcher.FindOne().Properties;



        //        if (prop.Contains("mail"))
        //        {
        //            employeeProfile.Email = prop["mail"][0].ToString();
        //        }

        //        if (prop.Contains("givenName"))
        //        {
        //            employeeProfile.FirstNameEn = prop["givenName"][0].ToString();
        //        }

        //        if (prop.Contains("displayname"))
        //        {
        //            employeeProfile.NameEn = prop["displayname"][0].ToString();
        //        }

        //        if (prop.Contains("department"))
        //        {
        //            employeeProfile.Department = prop["department"][0].ToString();
        //        }

        //        if (prop.Contains("ipPhone"))
        //        {
        //            employeeProfile.Extension = prop["ipPhone"][0].ToString();
        //        }
        //    }
        //    return employeeProfile;
        //}

    }
}