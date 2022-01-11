using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Award.Core.Entities;
using Award.Infrastructure.Data;
using Award.Web.Common.Utils;
using Award.Web.Models;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using static Award.Web.Models.CommonLookUp;

namespace Award.Web.Controllers
{
    [Authorize]

    public class UserDetailController : BaseController
    {
        private readonly AwardDbContext _context;

        private readonly IFileInfo _fileInfo;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IStringLocalizer<NotificationController> _localizer;

        public UserDetailController(AwardDbContext context, IFileInfo fileInfo, IHostingEnvironment hostEnv, IStringLocalizer<NotificationController> localizer) : base(context)
        {
            _context = context;
            _fileInfo = fileInfo;
            _hostEnv = hostEnv;
            _localizer = localizer;
        }
        public async Task<IActionResult> Index(long catId = 0, long saveStatus = 0, long sectorId = 0)
        {

            var commonLookUp = new CommonLookUp();

            var empDetail = (from emp in _context.Employee
                             join qSM in _context.QualitySectorManagers on emp.SectorId equals qSM.SectorId
                             join u in _context.Users on emp.Id equals u.IdEmployee
                             join ur in _context.UserRoles on u.Id equals ur.UserId 
                             join cS in _context.CategorySectorEntries on qSM.SectorId equals cS.SectorId
                             join s in _context.Sectors on emp.SectorId equals s.Id
                             join d in _context.Departments on emp.DepartmentId equals d.Id
                             join sec in _context.Sections on emp.SectionId equals sec.Id
                             join b in _context.Branches on emp.BranchId equals b.Id
                             join unit in _context.Units on emp.UnitId equals unit.Id
                             join qSME in _context.QSMEntries on new { X1 = u.Id, X2 = cS.CategoryId }
                             equals
                             new { X1 = qSME.AssignedUserId, X2 = qSME.CategoryId }
                             //on u.Id equals qSME.AssignedUserId 
                             //&& qSME.CategoryId equals cS.CategoryId 
                            into qUA
                             from qSME in qUA.DefaultIfEmpty()
                             where qSM.UserId == long.Parse(CurrentUserId) && s.Id == sectorId && cS.CategoryId == catId && ur.RoleId ==3
                             select new
                             {
                                 EmpId = emp.Id,
                                 NameEn = emp.NameEn,
                                 NameAr = emp.NameAr,
                                 eMail = u.Email,
                                 sector = CurrentLanguage == "en" ? s.NameEn : s.NameAr,
                                 dept = CurrentLanguage == "en" ? d.NameEn : d.NameAr,
                                 section = CurrentLanguage == "en" ? sec.NameEn : sec.NameAr,
                                 branch = CurrentLanguage == "en" ? b.NameEn : b.NameAr,
                                 unit = CurrentLanguage == "en" ? unit.NameEn : unit.NameAr,
                                 isAssigned = false,
                                 userId = u.Id,
                                 currentStatus = 0,
                                 Rank = CurrentLanguage == "en" ? emp.RankEn : emp.RankAr
                             }).ToList();

            List<UsersDetail> userDetail = new List<UsersDetail>();
            foreach (var emp in empDetail)
            {
                var curStatus = "";
                var qsmEntryDetail = _context.QSMEntries.Where(q => q.SubmittedUserId == long.Parse(CurrentUserId) && q.CategoryId == catId && q.AssignedUserId == emp.userId).FirstOrDefault() == null ? 0 : _context.QSMEntries.Where(q => q.SubmittedUserId == long.Parse(CurrentUserId) && q.CategoryId == catId && q.AssignedUserId == emp.userId).FirstOrDefault().CurrentStatusId;
                if (qsmEntryDetail > 0)
                {

                    curStatus = qsmEntryDetail == 2 ? "Accept/Return" : qsmEntryDetail == 3 ? "Waiting For Response" : qsmEntryDetail == 4 ? "Submitted" : "Assigned";

                }
                else
                {
                    curStatus = "View";
                }
                userDetail.Add(new UsersDetail { EmployeeId = emp.EmpId, Sector = emp.sector, EmployeeNameEn = emp.NameEn, EmployeeNameAr = emp.NameAr, Email = emp.eMail, Department = emp.dept, Section = emp.section, Branch = emp.branch, Unit = emp.unit, IsAssigned = qsmEntryDetail > 0 ? true : false, UserId = emp.userId, qsmEntryStatus = qsmEntryDetail, Rank = emp.Rank , CurrentStatus =curStatus});
            }
            commonLookUp.userDetail = userDetail;
            commonLookUp.CategoryId = catId;
            commonLookUp.saveStatus = saveStatus;
            commonLookUp.selectedSectorId = sectorId;

            commonLookUp.iSAssignButtonEnable =commonLookUp.userDetail.Any(q => q.qsmEntryStatus >0 &&  q.qsmEntryStatus<4);

            return View(commonLookUp);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommonLookUp commonLookUp)
        {
            var catId = commonLookUp?.CategoryId;
            var catName="";
            var award = "";
            try
            {
                foreach (var data in commonLookUp?.userDetail)
                {
                    var assignedUser = (from u in _context.Users
                                        where u.IdEmployee == data.EmployeeId
                                        select new
                                        {
                                            Id = u.Id
                                        }).FirstOrDefault();
                    if (assignedUser != null)
                    {
                        var isExist = (_context.QSMEntries.Where(q => q.AssignedUserId == assignedUser.Id && q.CategoryId == commonLookUp.CategoryId && q.CurrentStatusId !=4));

                        if (isExist?.Count() == 0 && data.IsAssigned == true)
                        {
                            QSMEntries qSMEntries = new QSMEntries();
                            qSMEntries.CreateDate = DateTime.Now;
                            qSMEntries.CategoryId = commonLookUp.CategoryId;
                            qSMEntries.CurrentStatusId = 1;
                            qSMEntries.AssignedUserId = assignedUser.Id;
                            qSMEntries.SubmittedUserId = long.Parse(CurrentUserId);
                            await _context.AddAsync(qSMEntries).ConfigureAwait(false);
                            await _context.SaveChangesAsync();


                            GetAwardDetail(commonLookUp.CategoryId,out award, out catName);
                            string notificationMessage = _localizer["SubmissionFromQSM"];
                            notificationMessage = notificationMessage.Replace("CategoryName", catName);
                            notificationMessage = notificationMessage.Replace("AwardName", award);
                            notificationMessage = notificationMessage.Replace("QSMUSER", CurrentUserFullName);

                            InsertNotification(assignedUser.Id, notificationMessage, 1);

                        }
                        if (isExist?.Count() > 0 && data.IsAssigned == false)
                        {

                            var result = (from q in _context.QSMEntries
                                          where q.AssignedUserId == assignedUser.Id && q.CategoryId == commonLookUp.CategoryId && q.CurrentStatusId != 2
                                          select q).SingleOrDefault();
                            if (result != null)
                            {
                                _context.QSMEntries.Remove(result);
                                await _context.SaveChangesAsync();
                            }
                            GetAwardDetail(commonLookUp.CategoryId, out award, out catName);
                            string notificationMessage = _localizer["CancelSubmissionFromQSM"];
                            notificationMessage = notificationMessage.Replace("CategoryName", catName);
                            notificationMessage = notificationMessage.Replace("AwardName", award);
                            notificationMessage = notificationMessage.Replace("QSMUSER", CurrentUserName);
                            InsertNotification(assignedUser.Id, notificationMessage, 1);

                        }
                    }
                }


                return RedirectToAction("Index", "UserDetail", new { catId = catId, saveStatus = 1, sectorId = commonLookUp.selectedSectorId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "UserDetail", new { catId = catId, saveStatus = 0 });

            }

        }

        private void GetAwardDetail(long catId,out string award, out string category)
        {
            var data = (from a in _context.Awards
                       join c in _context.Categories on a.Id equals c.IdAward
                        where c.Id ==catId 
                       select new
                       {
                         Award=  a.Name,
                         CatName=  c.Name 
                       }).FirstOrDefault();
            award = data.Award;
            category = data.CatName;

        }
        public FileResult DownloadSubCriteriaFiles(long subCriteria,long qsmEntryId=0)
        {
            var webRoot = _hostEnv.WebRootPath;
            var fileName = "myZip.zip";
            var tempOutPut = webRoot + "/temp/" + fileName;

            using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutPut)))
            {
                zipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];
                var ImageList = new List<string>();

                var awards = _context.CriteriaDocuments.Where(c => c.SubCriteriaId == subCriteria && c.QSMEntryId==qsmEntryId).ToList();
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

        public bool CheckIfAssignButtonShouldHide(long catId,long empUserId,string actionTaken)
        {
            var qsmEntryDetail = _context.QSMEntries.Where(q => 
                                                            q.SubmittedUserId == long.Parse(CurrentUserId) 
                                                         && q.CategoryId == catId
                                                         && q.AssignedUserId == empUserId 
                                                          )
             
                                          .FirstOrDefault();

            if(qsmEntryDetail!=null &&  actionTaken == "uncheck" && qsmEntryDetail.CurrentStatusId == 1)
            {
                return false;// show
            }
            if(qsmEntryDetail == null && actionTaken=="check")
            {
                return false;// show
            }
            else if (qsmEntryDetail == null && actionTaken == "uncheck")
            {
                return true; // hide
            }
            else if (qsmEntryDetail != null && actionTaken == "check")
            {
                return true; // hide
            }
            else
            {
                return true;
            }
            
        }
    }
}