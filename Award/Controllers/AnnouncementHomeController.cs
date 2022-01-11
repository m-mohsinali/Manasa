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

namespace Award.Web.Controllers
{
    [Authorize]
    public class AnnouncementHomeController : BaseController
    {
        private readonly AwardDbContext _context;
        private readonly IFileInfo _fileInfo;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly ILdapAuthenticationService _ldapAuthenticationService;
        public AnnouncementHomeController(AwardDbContext context, IFileInfo fileInfo, IWebHostEnvironment hostEnv, ILdapAuthenticationService ldapAuthenticationService) : base(context)
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
            return View(await _context.AnnouncementHome.ToListAsync());
        }

        // GET: Announcements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.AnnouncementHome
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
        public async Task<IActionResult> Create(AnnouncementHomeViewModel announcementVm)
        {
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            if (!ModelState.IsValid)
            {
                return View(announcementVm);
            }

            announcementVm.AnnouncementHome.ThumbnailImagePath = await _fileInfo
                .SaveUploadFile(_hostEnv.WebRootPath,
                    AppSetting.Instance.AnnouncementImagePath,
                                announcementVm.ThumbnailImage).ConfigureAwait(false);

            announcementVm.AnnouncementHome.Name = System.IO.Path.GetFileName(announcementVm.ThumbnailImage.FileName);
            announcementVm.AnnouncementHome.CreatedAt = DateTime.Now;
            _context.Add(announcementVm.AnnouncementHome);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction("Index");
        }

        // GET: Announcements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var announcementHome = await _context.AnnouncementHome.FindAsync(id);

            var announcementHomeViewModel = new AnnouncementHomeViewModel();
            announcementHomeViewModel.AnnouncementHome = announcementHome;

            if (announcementHome == null)
            {
                return NotFound();
            }
            return View(announcementHomeViewModel);
            ////await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var announcement = await _context.AnnouncementHome.FindAsync(id);
            //if (announcement == null)
            //{
            //    return NotFound();
            //}
            //return View(announcement);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnnouncementHomeViewModel announcementHomeModel)
        {
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            if (id != announcementHomeModel.AnnouncementHome.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(announcementHomeModel);
            }
            try
            {
                if (announcementHomeModel.ThumbnailImage != null)
                {
                    announcementHomeModel.AnnouncementHome.ThumbnailImagePath = await _fileInfo
                   .SaveUploadFile(_hostEnv.WebRootPath,
                       AppSetting.Instance.AnnouncementImagePath,
                                   announcementHomeModel.ThumbnailImage).ConfigureAwait(false);
                    announcementHomeModel.AnnouncementHome.Name = System.IO.Path.GetFileName(announcementHomeModel.ThumbnailImage.FileName);

                }
                //announcementVm.AnnouncementHome.Name = System.IO.Path.GetFileName(announcementVm.ThumbnailImage.FileName);
                //announcementVm.AnnouncementHome.CreatedAt = DateTime.Now;
                _context.Update(announcementHomeModel.AnnouncementHome);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementExists(announcementHomeModel.AnnouncementHome.Id))
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

            var announcement = await _context.AnnouncementHome
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
            var announcement = await _context.AnnouncementHome.FindAsync(id);
            _context.AnnouncementHome.Remove(announcement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncementExists(long id)
        {

            return _context.AnnouncementHome.Any(e => e.Id == id);
        }

       

    }
}