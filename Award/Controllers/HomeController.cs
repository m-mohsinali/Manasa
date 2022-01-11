using Award.Core.Entities;
using Award.Core.Interfaces;
using Award.Infrastructure.Data;
using Award.Models;
using Award.Web.Common.Auth;
using Award.Web.Models;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Award.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly AwardDbContext _context;
        private IEmailSender _emailSender;
        private IUserSession _userSession;
        [Obsolete]
        private IHostingEnvironment _hostingEnvironment;
        private IManasaEmployeeRepository _mansaRep;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ISignInManager _signInManager;


        [Obsolete]
        public HomeController(AwardDbContext context, IEmailSender emailsender, IUserSession userSession, IHostingEnvironment hostingEnvironment, IManasaEmployeeRepository manasaEmployeeRepository, IStringLocalizer<HomeController> localizer, ISignInManager signInManager) : base(context)
        {
            _context = context;
            //  _logger = logger;
            _emailSender = emailsender;
            _userSession = userSession;
            _hostingEnvironment = hostingEnvironment;
            _mansaRep = manasaEmployeeRepository;
            _localizer = localizer;
            _signInManager = signInManager;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        public IActionResult Index()
        {

            if (IsAllowedToViewDashboard())
            {
                return RedirectToAction(nameof(Opening));
            }
            
            var announcements = _context.Announcements.ToList();
            var winner = (from c in _context.WinnerAnnouncements
                          join usr in _context.Users on c.UserId equals usr.Id
                          join e in _context.Employee on usr.IdEmployee equals e.Id
                          join awa in _context.Awards on c.AwardId equals awa.Id
                          join cat in _context.Categories on c.CategoryId equals cat.Id
                          select new ShowWinnersViewModel()
                          {
                              EmployeeName = CurrentLanguage == "en" ? usr.FirstName : usr.LastName,
                              UserId = usr.Id,
                              EmployeeImage = c.ImagePath,
                              Award = awa.Name,
                              Category = cat.Name,
                              Description = c.Description,
                              ShowImage = c.IsShow,
                              Year = c.AwardYear,
                              Id = c.Id
                          }
                   ).ToList();

            HomeViewModel model = new HomeViewModel()
            {
                Announcements = announcements,
                AnnouncedWinners = winner
            };
            return View("index", model);
        }
        [Authorize]

        public IActionResult Opening(int roleId = 0)
        {

            var profilePic = HttpContext.Session.GetString("alreadyLoggedIn");
            if (string.IsNullOrEmpty(profilePic))
            {
                var empsig = "";//CurrentUserPic;
                var emproles = "";//CurrentUserRoles;

                var userRolesList = UserRolesList(roleId);
                foreach (var data in userRolesList)
                {
                    emproles = emproles == "" ? data.RoleName : emproles + "," + data.RoleName;
                    empsig = data.EmpImage;
                }
                if (!string.IsNullOrEmpty(empsig) && !string.IsNullOrEmpty(emproles))
                {
                    HttpContext.Session.SetString("empsig", empsig);
                    HttpContext.Session.SetString("emproles", emproles);
                    HttpContext.Session.SetString("alreadyLoggedIn", "1");
                }
                else
                {
                    //return RedirectToAction("~/Account/Signout");
                    //return RedirectToAction("Signout", "Account"); 
                      _signInManager.SignOutAsync();
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                    //return Redirect("~/Home/Index");


                }
            }

            

            var claims = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();

            var awards = _context.Awards.ToList();
            var categories = _context.Categories.ToList();
            var announcements = _context.Announcements.ToList();
            var announcementHome = _context.AnnouncementHome.ToList();
            var VideosHome = _context.VideosHome.ToList();

            foreach (var v in announcementHome)
            {
                v.ThumbnailImagePath = "..\\" + v.ThumbnailImagePath;
            }
            foreach (var v in VideosHome)
            {
                v.ThumbnailImagePath = "..\\" + v.ThumbnailImagePath;
            }

            HomeViewModel model = new HomeViewModel()
            {
                Announcements = announcements,
                Awards = awards,
                Categories = categories,
                AnnouncementHome = announcementHome,
                VideosHome = VideosHome
            };
            return View("Opening", model);
        }

        [Authorize]

        public PartialViewResult GetCategoriesByAwardTypeID(int awardTypeID)
        {

            List<Category> categories = new List<Category>();
            categories.AddRange(_context.Categories.ToList());
            categories.ForEach(c => c.Award = _context.Awards.SingleOrDefault(a => a.Id == c.IdAward));

            if (awardTypeID > 0)
                categories = categories.Where(t => (int)t.AwardType == awardTypeID).ToList();

            return PartialView("_AwardCategories", categories);
        }
        [Authorize]

        public PartialViewResult GetCategoriesByAwardID(int awardID)
        {

            List<Category> categories = new List<Category>();
            categories.AddRange(_context.Categories.ToList());
            categories.ForEach(c => c.Award = _context.Awards.SingleOrDefault(a => a.Id == c.IdAward));

            if (awardID > 0)
                categories = categories.Where(t => (int)t.IdAward == awardID).ToList();

            return PartialView("_AwardCategories", categories);
        }
        [Authorize]

        [HttpGet]
        public ActionResult AddIntrest(long id)
        {
            try
            {
                var msg = 0;
                var isExist = _context.CategoryEntries.Where(c => c.UserId == long.Parse(CurrentUserId) && c.CategoryId == id).ToList();

                if (isExist.Count == 0)
                {
                    var qsmRoles = _context.UserRoles.Where(c => c.RoleId == 2).ToList();
                    var category = _context.Categories.Where(c => c.Id == id).FirstOrDefault().Name;

                    CategoryEntry categoryEntry = new CategoryEntry();
                    categoryEntry.CreateDate = DateTime.Now;
                    categoryEntry.CategoryId = id;
                    categoryEntry.CurrentStatusId = 1;
                    categoryEntry.RoleId = 0;
                    categoryEntry.UserId = long.Parse(CurrentUserId);
                    categoryEntry.Comment = "";
                    _context.AddAsync(categoryEntry).ConfigureAwait(false);
                    //_context.SaveChangesAsync();
                    msg = 1;
                    //Saad send notification

                    foreach (var data in qsmRoles)
                    { 
                    InsertNotification(data.UserId, CurrentUserFullName+" has requested for participation of category("+ category +")", 9);

                    }



                }

                return Json(new { result = msg });
            }
            catch (Exception ex)
            {
                return Json(new { result = ex.Message });
            }
        }
        [Authorize]
        public IActionResult AwardDetails(long AwardId)
        {
            AwardsViewModel awardViewModel = getAwardDetails(AwardId);
            return View("AwardDetails", awardViewModel);
        }
        [Authorize]
        public IActionResult CategoryDetails(long categoryId)
        {
            AwardsViewModel awardViewModel = getCategoryDetails(categoryId);
            return View("CategoryDetail", awardViewModel);
        }
        [Authorize]
        public ActionResult ShowMeetingInvitation(int userId)
        {
            var model = new MeetingInvitationViewModel();
            model.ToUserId = userId;
            return PartialView("_MeetingInvitation", model);
        }
        [Authorize]
        [HttpPost]
        public IActionResult SetMeeting(MeetingInvitationViewModel model)
        {
            var meetingModel = new MeetingInvitationViewModel();
            var msg = "";
            if (string.IsNullOrEmpty(model.EventDescription))
            {
                msg = _localizer["PleaseInsertMeetingDescription"];
            }
            if (model.StartTime == null)
            {
                msg = _localizer["PleaseInsertMeetingStartTime"];

            }
            if (model.StartTime < DateTime.Now)
            {
                msg = _localizer["StartTimeMsgLessThanTodayMessage"];

            }
            if (model.EndTime == null)
            {
                msg = _localizer["PleaseInsertMeetingEndTime"];

            }
            if (model.StartTime >= model.EndTime)
            {
                msg = _localizer["StartTimeMsg"];

            }

            model.ErrorMessage = msg;
            if (msg == "")
            {
                string response = InsertUserEvents(long.Parse(CurrentUserId), model.ToUserId, model.EventDescription, model.StartTime, model.EndTime);
                if (response == "")
                    model.SaveStatus = 1;
                AuditorNotificationForMeeting(model.ToUserId);
            }
            else
            {
                model.SaveStatus = 2;

            }

            return PartialView("_MeetingInvitation", model);

        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public AwardsViewModel getAwardDetails(long awardId)
        {
            AwardsViewModel awardViewModel = new AwardsViewModel();
            var award = _context.Awards.Where(d => d.Id == awardId).FirstOrDefault();



            var categories = _context.Categories.Where(a => a.IdAward == award.Id).ToList();
            awardViewModel.award = award;
            awardViewModel.Categories = categories;
            foreach (var cat in awardViewModel.Categories)
            {
                cat.CategoryCriteria = _context.CategoryCriterias.Where(d => d.CategoryId == cat.Id).ToList();
                cat.CategoryDocuments = _context.CategoryDocuments.Where(d => d.CategoryId == cat.Id).ToList();

                foreach (var cri in cat.CategoryCriteria)
                {
                    cri.CategorySubCriteria = _context.CategorySubCriterias.Where(d => d.CategoryCriteriaId == cri.Id).ToList();
                    cri.CategoryCriteriaDocuments = _context.CategoryCriteriaDocuments.Where(d => d.IdCategoryCriteria == cri.Id).ToList();
                }
            }
            return awardViewModel;
        }
        public AwardsViewModel getCategoryDetails(long categoryId)
        {
            AwardsViewModel awardViewModel = new AwardsViewModel();
            var category = _context.Categories.Where(d => d.Id == categoryId).FirstOrDefault();

            if (category != null)
            {
                var award = _context.Awards.Where(d => d.Id == category.IdAward).FirstOrDefault();
                awardViewModel.award = award;
                awardViewModel.category = category;
                awardViewModel.category.CategoryCriteria = _context.CategoryCriterias.Where(d => d.CategoryId == categoryId).ToList();
                foreach (var cri in awardViewModel.category.CategoryCriteria)
                {
                    cri.CategorySubCriteria = _context.CategorySubCriterias.Where(d => d.CategoryCriteriaId == cri.Id).ToList();
                    cri.CategoryCriteriaDocuments = _context.CategoryCriteriaDocuments.Where(d => d.IdCategoryCriteria == cri.Id).ToList();
                }
            }
            return awardViewModel;
        }
        [Authorize]
        public FileResult DownloadCategoryFiles(long categoryId)
        {
            var webRoot = _hostingEnvironment.WebRootPath;
            var fileName = "myZip.zip";
            var tempOutPut = webRoot + "/temp/" + fileName;

            using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutPut)))
            {
                zipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];
                var ImageList = new List<string>();

                var catDocs = _context.CategoryDocuments.Where(c => c.Category.Id == categoryId).ToList();
                foreach (var cat in catDocs)
                {
                    ImageList.Add(webRoot + "/" + cat.SupportingDocumentPath);
                }
                // ImageList.Add(webRoot+ "/Document/Category/1d89073f-220b-4b9e-9212-80ab9debc6e9.txt");
                // ImageList.Add(webRoot + "/Document/Category/fca94ba6-33b9-41e0-ae4f-fbc6e3426132.pdf");

                foreach (var file in ImageList)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    zipOutputStream.PutNextEntry(entry);

                    if (System.IO.File.Exists(file))
                    {
                        using (FileStream fileStream = System.IO.File.OpenRead(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                                zipOutputStream.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }

                }

                zipOutputStream.Finish();
                zipOutputStream.Flush();
                zipOutputStream.Close();
            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPut);
            if (System.IO.File.Exists(tempOutPut))
            {
                System.IO.File.Delete(tempOutPut);
            }
            if (finalResult == null || !finalResult.Any())
            {
                throw new Exception(string.Format("Nothing Found"));
            }

            return File(finalResult, "application/zip", fileName);
        }
        [Authorize]

        public FileResult DownloadAwardFiles(long awardId)
        {
            var webRoot = _hostingEnvironment.WebRootPath;
            var fileName = "myZip.zip";
            var tempOutPut = webRoot + "/temp/" + fileName;

            using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutPut)))
            {
                zipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];
                var ImageList = new List<string>();

                var awards = _context.AwardDocuments.Where(c => c.Awards.Id == awardId).ToList();
                foreach (var aw in awards)
                {
                    ImageList.Add(webRoot + "/" + aw.SupportingDocumentPath);
                }
                // ImageList.Add(webRoot+ "/Document/Category/1d89073f-220b-4b9e-9212-80ab9debc6e9.txt");
                // ImageList.Add(webRoot + "/Document/Category/fca94ba6-33b9-41e0-ae4f-fbc6e3426132.pdf");

                foreach (var file in ImageList)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    zipOutputStream.PutNextEntry(entry);

                    if (System.IO.File.Exists(file))
                    {
                        using (FileStream fileStream = System.IO.File.OpenRead(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                                zipOutputStream.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                }

                zipOutputStream.Finish();
                zipOutputStream.Flush();
                zipOutputStream.Close();
            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPut);
            if (System.IO.File.Exists(tempOutPut))
            {
                System.IO.File.Delete(tempOutPut);
            }
            if (finalResult == null || !finalResult.Any())
            {
                throw new Exception(string.Format("Nothing Found"));
            }

            return File(finalResult, "application/zip", fileName);
        }
        [Authorize]

        public ActionResult print(long categoryid)
        {


            AwardsViewModel awardViewModel = getCategoryDetails(categoryid);
            awardViewModel.MediaType = "Print";
            var report = new ViewAsPdf("CategoryDetail", awardViewModel);
            return report;
        }

        public ActionResult Winners(string Year = "2019")
        {
            ViewData["AwardYearSelectList"] = new SelectList(Years(), "Text", "Value", Year);

            var winnerAnnouncement =
                  (from w in _context.WinnerAnnouncements
                   join u in _context.Users on w.UserId equals u.Id
                   join e in _context.Employee on u.IdEmployee equals e.Id
                   join awa in _context.Awards on w.AwardId equals awa.Id
                   join cat in _context.Categories on w.CategoryId equals cat.Id
                   where w.AwardYear == Year
                   select w)
                   .Include(d => d.User)
                   .Include(d => d.User.Employee)
                   .Include(d => d.Awards)
                   .Include(d => d.Category)
                   .ToList();
            return View(winnerAnnouncement);
        }
        private bool IsAllowedToViewDashboard()
        {
            return _userSession.IsAuthenticated &&
                (
                _userSession.Isadmin
                || _userSession.IsQSM
                || _userSession.IsEndUser
                || _userSession.IsAuditManager
                || _userSession.IsAuditor
                || _userSession.IsJury
                || _userSession.IsSuperAdmin

                );
        }
        private void AuditorNotificationForMeeting(long userId)
        {
            string notificationMessage = _localizer["MeetingScheduleInvitation"];
            notificationMessage = notificationMessage.Replace("AuditorName", CurrentUserFullName);
            InsertNotification(userId, notificationMessage, 8);
        }

        [HttpGet]
        public IActionResult AnnouncementDetails(long Id)
        {
            var announcement = _context.Announcements.Where(cse => cse.Id == Id).FirstOrDefault();

            return View(announcement);
        }


        [HttpGet]
        public IActionResult WinnerDetails(long Id)
        {
            var winner = (from c in _context.WinnerAnnouncements
                          join usr in _context.Users on c.UserId equals usr.Id
                          where c.Id == Id
                          select new ShowWinnersViewModel()
                          {
                              EmployeeName = CurrentLanguage == "en" ? usr.FirstName : usr.LastName,
                              UserId = usr.Id,
                              EmployeeImage = c.ImagePath,
                              Award = c.AwardName,
                              Category = c.CategoryName,
                              Description = c.Description,
                              ShowImage = c.IsShow,
                              Id = c.Id
                          }
                             ).FirstOrDefault();

            return View(winner);
        }


        [HttpGet]
        public ActionResult LoadAnnouncementDetail(long id)
        {
            var announcement = _context.Announcements.Where(cse => cse.Id == id).ToList();
            List<Announcement> result = new List<Announcement>();

            for (int i = 0; i < announcement.Count; i++)
            {
                result.Add(new Announcement { Id = announcement[i].Id, Name = announcement[i].Title, Description = announcement[i].Description, StartAnnouncingDate = announcement[i].StartAnnouncingDate, EndAnnouncingDate = announcement[i].EndAnnouncingDate });
            }
            return Json(new { result = result });
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

    }
}
