using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Award.Core.Entities;
using Award.Infrastructure.Data;
using Award.Web.Models;
using Award.Web.Common.Utils;
using Microsoft.AspNetCore.Hosting;
using Award.Core.Common;
using Microsoft.AspNetCore.Authorization;

namespace Award.Web.Controllers
{
    [Authorize]

    public class WinnerAnnouncementsController : BaseController
    {
        private readonly AwardDbContext _context;
        private readonly IFileInfo _fileInfo;
        private readonly IWebHostEnvironment _hostEnv;

        public WinnerAnnouncementsController(AwardDbContext context, IFileInfo fileInfo, IWebHostEnvironment hostEnv) : base(context)
        {
            _context = context;
            _fileInfo = fileInfo;
            _hostEnv = hostEnv;
        }

        // GET: WinnerAnnouncements
        public async Task<IActionResult> Index()
        {
            var winnerAnnouncement =
                  (from w in _context.WinnerAnnouncements
                   join u in _context.Users on w.UserId equals u.Id
                   join e in _context.Employee on u.IdEmployee equals e.Id
                   join awa in _context.Awards on w.AwardId equals awa.Id
                   join cat in _context.Categories on w.CategoryId equals cat.Id
                   select w)
                   .Include(d => d.User)
                   .Include(d => d.User.Employee)
                   .Include(d => d.Awards)
                   .Include(d => d.Category)
                   .ToListAsync();

            return View(await winnerAnnouncement);
        }

        // GET: WinnerAnnouncements/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var winnerAnnouncement =
                     (  from w in _context.WinnerAnnouncements
                       join u in _context.Users on w.UserId equals u.Id
                       join e in _context.Employee on u.IdEmployee equals e.Id
                       where w.Id == id
                       select w).FirstOrDefault();

         //   var winnerAnnouncement = await _context.WinnerAnnouncements
               // .FirstOrDefaultAsync(m => m.Id == id);
            if (winnerAnnouncement == null)
            {
                return NotFound();
            }

            return View(winnerAnnouncement);
        }

        // GET: WinnerAnnouncements/Create
        public IActionResult Create()
        {
            fillDropdowns("", "", "");
            return View();
        }

        public void fillDropdowns(string awardid,string categoryid, string year)
        {
            var Awards = _context.Awards.Select(r => new { r.Id, r.Name }).ToList();
            var Categories = _context.Categories.Select(r => new { r.Id, r.Name }).ToList();


            ViewData["AwardSelectList"] = new SelectList(Awards, "Id", "Name", awardid);
            ViewData["CategoriesSelectList"] = new SelectList(Categories, "Id", "Name", awardid);
            ViewData["AwardYearSelectList"] = new SelectList(Years(), "Text", "Value", year);
        }
        private IEnumerable<SelectListItem> Years()
        {
              return new SelectListItem[]
              {
                  new SelectListItem() { Text = "2015", Value = "2015" },
                  new SelectListItem() { Text = "2016", Value = "2016" },
                  new SelectListItem() { Text = "2017", Value = "2017" },
                  new SelectListItem() { Text = "2018", Value = "2018" },
                  new SelectListItem() { Text = "2019", Value = "2019" },
                  new SelectListItem() { Text = "2020", Value = "2020" },
                  new SelectListItem() { Text = "2021", Value = "2021" }
              };
        }
        private IEnumerable<string> Years1()
        {
            return new List<string>()
            {"2015","2016","2017","2018","2018","2019","2020","2021" };
        }


        // POST: WinnerAnnouncements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( WinnerAnnouncementViewModel winAnnouncement)
        {
            if (!ModelState.IsValid)
            {
                return View(winAnnouncement);
            }
           // winAnnouncement.winnerAnnouncement.ImagePath = await _fileInfo.SaveUploadFile(_hostEnv.WebRootPath, AppSetting.Instance.AwardImagePath, winAnnouncement.winnerImagePath).ConfigureAwait(false);
            winAnnouncement.winnerAnnouncement.CreateDate = DateTime.Now;
            _context.Add(winAnnouncement.winnerAnnouncement);
            // _context.Add(award);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        // GET: WinnerAnnouncements/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var winnerAnnouncement =
                     (from w in _context.WinnerAnnouncements
                      join u in _context.Users on w.UserId equals u.Id
                      join e in _context.Employee on u.IdEmployee equals e.Id
                      join awa in _context.Awards on w.AwardId equals awa.Id
                      join cat in _context.Categories on w.CategoryId equals cat.Id
                      where w.Id == id
                      select w).Include(d=>d.User).Include(d=>d.User.Employee).Include(d => d.Awards)
                   .Include(d => d.Category).FirstOrDefault();

            if (winnerAnnouncement == null)
            {
                return NotFound();
            }

            fillDropdowns(winnerAnnouncement.AwardId?.ToString(), winnerAnnouncement.CategoryId?.ToString(), winnerAnnouncement.AwardYear?.ToString());
            return View(winnerAnnouncement);
        }

        // POST: WinnerAnnouncements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AwardId,CategoryId,AwardYear,AwardName,CategoryName,UserId,Description,AnnouncementDate,ImagePath,IsShow,Id,CreateDate,UpdatedDate,DeletedDate")] WinnerAnnouncement winnerAnnouncement)
        {
            if (id != winnerAnnouncement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(winnerAnnouncement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WinnerAnnouncementExists(winnerAnnouncement.Id))
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
            return View(winnerAnnouncement);
        }

        // GET: WinnerAnnouncements/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var winnerAnnouncement =
                     (from w in _context.WinnerAnnouncements
                      join u in _context.Users on w.UserId equals u.Id
                      join e in _context.Employee on u.IdEmployee equals e.Id
                      join awa in _context.Awards on w.AwardId equals awa.Id
                      join cat in _context.Categories on w.CategoryId equals cat.Id
                      where w.Id == id
                      select w).Include(d => d.User).Include(d => d.User.Employee).Include(d => d.Awards)
                   .Include(d => d.Category).FirstOrDefault();

            if (winnerAnnouncement == null)
            {
                return NotFound();
            }

            return View(winnerAnnouncement);
        }

        // POST: WinnerAnnouncements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var winnerAnnouncement = await _context.WinnerAnnouncements.FindAsync(id);
            _context.WinnerAnnouncements.Remove(winnerAnnouncement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WinnerAnnouncementExists(long id)
        {
            return _context.WinnerAnnouncements.Any(e => e.Id == id);
        }
        public JsonResult getEmployee(string q)
        {
            var user = (from u in _context.Users
                          join e in _context.Employee on u.IdEmployee equals e.Id
                          where e.UserDomain == q || e.NameEn ==q || e.NameAr ==q
                          select new UsersDetail()
                          {
                              UserId = u.Id,
                              Name = CurrentLanguage == "en" ? e.NameEn : e.NameAr,
                              EmployeeNameEn = e.NameEn ,
                              EmployeeNameAr = e.NameAr,
                              JobEn = e.JobEn,
                              JobAr = e.JobAr,
                              RankEn = e.RankEn,
                              RankAr = e.RankAr,
                              Email =  u.Email,
                              EmployeeImageURL = e.EmployeePhotoUrl,
                              Group=e.Grp 
                          }
                             ).FirstOrDefault();
            return Json(user);

        }

        public async Task<IActionResult> AnnounceWinner(long catId, long winnerUserId, long awardId, string desc, long year)
        {

            try
            {
                var isAlreadyExist = _context.WinnerAnnouncements.Where(x => x.UserId == winnerUserId && x.CategoryId == catId && x.AwardId == awardId).Count();

                if (isAlreadyExist == 0)
                {
                    var winnerAnnouncement = new WinnerAnnouncement();
                    winnerAnnouncement.CreateDate = DateTime.Now;
                    winnerAnnouncement.CategoryId = catId;
                    winnerAnnouncement.AwardId = awardId;
                    winnerAnnouncement.UserId = winnerUserId;
                    winnerAnnouncement.IsShow = true;
                    winnerAnnouncement.Description = desc;
                    winnerAnnouncement.AnnouncementDate = DateTime.Now;
                    winnerAnnouncement.ImagePath = _context.Users.Where(u => u.Id == winnerUserId).Include(e => e.Employee).FirstOrDefault().Employee.EmployeePhotoUrl ;
                    winnerAnnouncement.AwardYear = year.ToString();


                    await _context.AddAsync(winnerAnnouncement).ConfigureAwait(false);
                    await _context.SaveChangesAsync();
                }

                //else
                //{
                //    if (isAlreadyExist == 0 && isAllocated == false)
                //    {
                //        CategorySectorEntry categorySectorEntry = new CategorySectorEntry();
                //        categorySectorEntry.CreateDate = DateTime.Now;
                //        categorySectorEntry.CategoryId = catId;
                //        categorySectorEntry.CurrentStatusId = 1;
                //        categorySectorEntry.SectorId = sectorId;

                //        await _context.AddAsync(categorySectorEntry).ConfigureAwait(false);
                //        await _context.SaveChangesAsync();

                //    }

                //}

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("Opening", "");
        }

    }
}
