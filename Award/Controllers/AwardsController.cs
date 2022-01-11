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
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Award.Core.Common;
using Award.Web.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace Award.Web.Controllers
{
    [Authorize]
    public class AwardsController : BaseController
    {
        private readonly AwardDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IFileInfo _fileInfo;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IStringLocalizer<AwardsController> _localizer;

        public AwardsController(AwardDbContext context, IWebHostEnvironment hostEnvironment, IFileInfo fileInfo, IWebHostEnvironment hostEnv, IStringLocalizer<AwardsController> localizer) : base(context)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            _fileInfo = fileInfo;
            _hostEnv = hostEnv;
            _localizer = localizer;
        }



        // GET: Awards
        public async Task<IActionResult> Index(string searchString)
        {
            var award = from m in _context.Awards
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                award = award.Where(s => s.Name.Contains(searchString));
            }

            var k = _localizer["Create Award"];

            //string imageDataURL = string.Format("data:image/png;base64,{0}", award.AwardImagePath);
            //ViewBag.ImageData = imageDataURL;

            return View(await award.ToListAsync());
            // return View(await _context.Awards.ToListAsync());
        }


        // GET: Awards/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var awards = await _context.Awards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (awards == null)
            {
                return NotFound();
            }
            return View(awards);
        }

        // GET: Awards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Awards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Name,Description,AwardImagePath,DocumentPath,OpeningDate,ClosingDate,Id,CreateDate,UpdatedDate,DeletedDate")] Awards awards)
        //public async Task<IActionResult> Create([Bind("Name,Description,AwardImagePath,DocumentPath,OpeningDate,ClosingDate,Id,CreateDate,UpdatedDate,DeletedDate")] Awards awards)
        //public async Task<IActionResult> Create([Bind("Name,Description,AwardImagePath,DocumentPath,OpeningDate,ClosingDate,Id,CreateDate,UpdatedDate,DeletedDate")] AwardsViewModel awardsvm, IFormFile formFiles, List<IFormFile> formFiles2)
        public async Task<IActionResult> Create(AwardsViewModel awardsvm)
        {
            if (!ModelState.IsValid)
            {
                return View(awardsvm);
            }
            // var imageContent = awardsvm.awardImagePath.ContentType;

            //var memoryStream = new MemoryStream();
            //_ = awardsvm.awardImagePath.CopyToAsync(memoryStream);
            //awardsvm.award.AwardImagePath = "data:" + awardsvm.awardImagePath.ContentType.ToString() + ";base64," + Convert.ToBase64String(memoryStream.ToArray());
            //  awardsvm.award.AwardImagePath = base64String;
            // byte[] bytes = Convert.FromBase64String(base64String);

            awardsvm.award.AwardImagePath = await _fileInfo.SaveUploadFile(_hostEnv.WebRootPath, AppSetting.Instance.AwardImagePath, awardsvm.awardImagePath).ConfigureAwait(false);

            awardsvm.award.AwardDocuments = new List<AwardDocuments>();

            if (awardsvm.AwardDocuments != null)
            {
                foreach (var uploadDoc in awardsvm.AwardDocuments)
                {
                    var AwardDocuments = new AwardDocuments
                    {
                        SupportingDocumentPath = await _fileInfo
                            .SaveUploadFile(_hostEnv.WebRootPath, AppSetting.Instance.AwardDocumentPath,
                                uploadDoc).ConfigureAwait(false)
                    };
                    awardsvm.award.AwardDocuments.Add(AwardDocuments);
                }
            }



            awardsvm.award.CreateDate = DateTime.Now;
            awardsvm.award.AwardStatus = AwardStatus.Open;
            _context.Add(awardsvm.award);
            // _context.Add(award);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }
        //public byte[] GetBytes(this IFormFile formFile)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        formFile.CopyToAsync(memoryStream);
        //        return memoryStream.ToArray();
        //    }
        //}


        public async Task<IActionResult> Publish(AwardsViewModel awardsvm)
        {
            if (!ModelState.IsValid)
            {
                return View(awardsvm.award);
            }


            awardsvm.award.AwardImagePath = await _fileInfo.SaveUploadFile(_hostEnv.WebRootPath, AppSetting.Instance.AwardImagePath, awardsvm.awardImagePath).ConfigureAwait(false);

            awardsvm.award.AwardDocuments = new List<AwardDocuments>();
            foreach (var uploadDoc in awardsvm.AwardDocuments)
            {
                var AwardDocuments = new AwardDocuments
                {
                    SupportingDocumentPath = await _fileInfo
                        .SaveUploadFile(_hostEnv.WebRootPath, AppSetting.Instance.AwardDocumentPath,
                            uploadDoc).ConfigureAwait(false)
                };
                awardsvm.award.AwardDocuments.Add(AwardDocuments);
            }


            awardsvm.award.CreateDate = DateTime.Now;
            awardsvm.award.AwardStatus = AwardStatus.New;
            _context.Add(awardsvm.award);
            // _context.Add(award);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));

        }

        // GET: Awards/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var awards = await _context.Awards.FindAsync(id);
            var AwardsViewModel = new AwardsViewModel
            {
                award = awards
            };
            if (awards == null)
            {
                return NotFound();
            }
            return View(AwardsViewModel);
        }

        // POST: Awards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        //public async Task<IActionResult> Edit(long id, [Bind("Name,Description,AwardStatus,AwardImagePath,DocumentPath,OpeningDate,ClosingDate,Id,CreateDate,UpdatedDate,DeletedDate")] Awards awards)
        public async Task<IActionResult> Edit(long id, AwardsViewModel awardsvm)
        {
            if (ModelState.IsValid)
            {
                if (awardsvm.awardImagePath != null)
                    awardsvm.award.AwardImagePath = await _fileInfo.SaveUploadFile(_hostEnv.WebRootPath, AppSetting.Instance.AwardImagePath, awardsvm.awardImagePath).ConfigureAwait(false);

                if (awardsvm.AwardDocuments != null)
                {
                    foreach (var uploadDoc in awardsvm.AwardDocuments)
                    {
                        var AwardDocuments = new AwardDocuments
                        {
                            SupportingDocumentPath = await _fileInfo
                                .SaveUploadFile(_hostEnv.WebRootPath, AppSetting.Instance.AwardDocumentPath,
                                    uploadDoc).ConfigureAwait(false)
                        };
                        awardsvm.award.AwardDocuments.Add(AwardDocuments);
                    }
                }


                awardsvm.award.Id = id;
                try
                {
                    awardsvm.award.UpdatedDate = DateTime.Now;
                    _context.Update(awardsvm.award);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AwardsExists(awardsvm.award.Id))
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
            return View(awardsvm);
        }

        // GET: Awards/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var awards = await _context.Awards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (awards == null)
            {
                return NotFound();
            }

            return View(awards);
        }

        public void getimagefrompath(string X)
        {

            // TestDatePhoto testDatePhoto = db.TestDatePhoto.Find(id);
            if (X == null)
            {
                ViewBag.ImageData = "Image not found ..!";
            }
            else
            {
                //    Convert.ToBase64String()
                //string imageBase64Data = Convert.ToBase64String(testDatePhoto.Photo);
                //string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                ViewBag.ImageData = "";
            }
        }

        // POST: Awards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var awards = await _context.Awards.FirstAsync(a => a.Id == id);
            _context.Awards.Remove(awards);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public FileResult Download()
        {
            List<string> files = new List<string>() { "Document\\Award\\test.pdf" };
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
        private bool AwardsExists(long id)
        {
            return _context.Awards.Any(e => e.Id == id);
        }
    }
}
