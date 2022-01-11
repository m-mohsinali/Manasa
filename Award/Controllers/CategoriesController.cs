using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Award.Core.Entities;
using Award.Infrastructure.Data;
using Award.Web.Common.Utils;
using Microsoft.AspNetCore.Hosting;
using Award.Web.Models;
using Award.Core.Common;
using Award.Web.Common.Extensions;
using System.Text;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Award.Web.Controllers
{
    [Authorize]
    public class CategoriesController : BaseController
    {
        private readonly AwardDbContext _context;

        private readonly IFileInfo _fileInfo;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IStringLocalizer<NotificationController> _localizer;

        public CategoriesController(AwardDbContext context, IFileInfo fileInfo, IHostingEnvironment hostEnv, IStringLocalizer<NotificationController> localizer) : base(context)
        {
            _context = context;
            _fileInfo = fileInfo;
            _hostEnv = hostEnv;
            _localizer = localizer;

        }

        // GET: Categoriesfinto
        public async Task<IActionResult> Index()
        {
            TempData["EndUserSubmitButton"] = "";
            var qsmAssignViewModel = new List<QsmAssignViewModel>();
            // ViewBag.Users = _context.Users.ToList();
            Category cat = new Category();
            //var rr = TempData["CategorySuccessMsg"];
            var categories = new List<Category>();// await _context.Categories.ToListAsync().ConfigureAwait(false);
            if (IsRolePresent("EndUser"))
            {
                categories = await _context.Categories.ToListAsync().ConfigureAwait(false);
                categories = categories.Where(c => _context.QSMEntries.Any(qsm => qsm.CategoryId == c.Id && qsm.AssignedUserId == long.Parse(CurrentUserId))).ToList();

            }
            if (IsRolePresent("QualitySectionManager"))
            {
                var categoriesDetail = (from c in _context.Categories
                                        join cse in _context.CategorySectorEntries on c.Id equals cse.CategoryId
                                        join qsm in _context.QualitySectorManagers on cse.SectorId equals qsm.SectorId
                                        join s in _context.Sectors on cse.SectorId equals s.Id
                                        join a in _context.Awards on c.IdAward equals a.Id
                                        where qsm.UserId == long.Parse(CurrentUserId)
                                        select new QsmAssignViewModel()
                                        {
                                            CatId = c.Id,
                                            Name = c.Name,
                                            AwardName = a.Name,
                                            AwardTypeNew = c.AwardType,
                                            OpeningDate = a.OpeningDate,
                                            ClosingDate = a.ClosingDate,
                                            SectorId = s.Id,
                                            SectorName = CurrentLanguage == "en" ? s.NameEn : s.NameAr

                                        }
                                ).ToList();

                ViewBag.data = categoriesDetail;

                //categories.Where(c => _context.CategorySectorEntries.Any(q => c.Id == q.CategoryId && q.SectorId  == long.Parse(CurrentUserId))).ToList();
            }


            if (IsRolePresent("Administrator"))
            {
                categories = await _context.Categories.OrderBy(o => o.CreateDate).ToListAsync().ConfigureAwait(false);
                //categories = (List<Category>)categories.OrderBy(o => o.CreateDate);

                //categories1 = await _context.Categories.ToListAsync().ConfigureAwait(false);


            }
            categories.ForEach(c => c.Award = _context.Awards.SingleOrDefault(a => a.Id == c.IdAward));

            var data = (from qsm in _context.QSMEntries
                        join ua in _context.UserAnswers on qsm.Id equals ua.QsmEntryId
                        where (qsm.AssignedUserId == long.Parse(CurrentUserId) || qsm.SubmittedUserId == long.Parse(CurrentUserId)) && ua.CurrentStatusId == 2
                        select new
                        {
                            Catid = qsm.CategoryId,
                            CurrentStatusId=qsm.CurrentStatusId 
                        }).ToList();
            if (data != null && data.Count() > 0)
            {
                for (int i = 0; i < categories.Count(); i++)
                {
                    for (int j = 0; j < data.Count(); j++)
                    {
                        if (categories[i].Id == data[j].Catid)
                        {
                            categories[i].CurrentStatusId = data[j].CurrentStatusId;
                        }
                    }
                }
            }

            //if (IsRolePresent("EndUser"))
            //{
            //    for (int i = 0; i < categories.Count; i++)
            //    {
            //        var t = _context.QSMEntries.Any(qsm => qsm.CategoryId == categories[i].Id && qsm.AssignedUserId == long.Parse(CurrentUserId) && qsm.CurrentStatusId == 3);
            //        if (t == true)
            //            categories[i].CurrentStatusId = 3;
            //    }
            //}

            ViewBag.Sectors = null;// _context.Sectors.ToList();
            return View(categories);
        }

        // GET: Categories/Details/5S
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Award)
                .Include(c => c.CategoryDocuments)
                .Include(c => c.CategoryCriteria).ThenInclude(c => c.CategorySubCriteria)
                .FirstOrDefaultAsync(m => m.Id == id);
            category.Award = _context.Awards.SingleOrDefault(a => a.Id == category.IdAward);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);

        }

        // GET: Categories/Create
        //public  IActionResult Create()
        public async Task<IActionResult> Create()
        {
            var categoryViewModel = new CategoryViewModel
            {
                Awards = await _context.Awards.ToListAsync().ConfigureAwait(false),
                AwardsType = await _context.AwardsType.ToListAsync().ConfigureAwait(false)
            };


            return View(categoryViewModel);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdAward,Name,Description,AwardType,OpeningDate,ClosingDate,Id,CreateDate,UpdatedDate,DeletedDate")] Category category)
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
        {
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);Saad
            if (!ModelState.IsValid)
            {
                categoryViewModel.Awards = await _context.Awards.ToListAsync().ConfigureAwait(false);
                categoryViewModel.AwardsType = await _context.AwardsType.ToListAsync().ConfigureAwait(false);
                return View(categoryViewModel);
            }
            categoryViewModel.Category.CategoryDocuments = new List<CategoryDocument>();
            if (categoryViewModel.CategoryDocuments != null)
            {
                foreach (var uploadDoc in categoryViewModel.CategoryDocuments)
                {
                    var categoryDocument = new CategoryDocument
                    {
                        SupportingDocumentPath = await _fileInfo
                            .SaveUploadFile(_hostEnv.WebRootPath, AppSetting.Instance.CategoryDocumentPath,
                                uploadDoc).ConfigureAwait(false),
                        DocumentName = uploadDoc.FileName
                    };
                    categoryViewModel.Category.CategoryDocuments.Add(categoryDocument);
                }
            }

            try
            {

                categoryViewModel.Category.CreateDate = DateTime.Now;
                await _context.AddAsync(categoryViewModel.Category).ConfigureAwait(false);
                await _context.SaveChangesAsync();
                TempData["CategorySuccessMsg"] = "Category Added Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                categoryViewModel.Awards = await _context.Awards.ToListAsync().ConfigureAwait(false);
            }

            return View(categoryViewModel);

            //if (ModelState.IsValid)
            //{
            //    _context.Add(category);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id).ConfigureAwait(false);
            if (category == null)
            {
                return NotFound();
            }

            var categoryViewModel = new CategoryViewModel
            {
                Category = category,
                Awards = await _context.Awards.ToListAsync().ConfigureAwait(false),
                AwardsType = await _context.AwardsType.ToListAsync().ConfigureAwait(false)

            };

            var categoryDocuments = await _context.CategoryDocuments.Where(c => c.Category.Id == category.Id)
                .ToListAsync()
                .ConfigureAwait(false);

            if (categoryDocuments != null && categoryDocuments.Count > 0)
            {
                categoryViewModel.Category.CategoryDocuments = categoryDocuments;
            }

            var categoryCriterias = await _context.CategoryCriterias.Where(c => c.IdCategory == category.Id).Include(c => c.CategorySubCriteria)
                .ToListAsync()
                .ConfigureAwait(false);
            if (categoryCriterias != null && categoryCriterias.Count > 0)
            {
                categoryViewModel.Category.CategoryCriteria = categoryCriterias;
            }
            return View(categoryViewModel);

        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, CategoryViewModel category, CategoryCriteria ct)
        {
            //if (ModelState.IsValid)
            //{
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var oldCategoryCritireaList = _context.CategoryCriterias.Where(a => a.Category.Id == id).ToList();
                        _context.CategoryCriterias.RemoveRange(oldCategoryCritireaList);

                        //Saad

                        // var oldSubCategoryCritireaList = _context.CategorySubCriterias.Where(item =>_context.CategorySubCriterias.Any(category => category.CategoryCriteriaId.Equals(item.CategoryCriteriaId)));



                        //var oldSubCategoryCritireaList = _context.CategorySubCriterias.Where(a => a.CategoryCriteriaId.any).ToList();
                        //_context.CategoryCriterias.RemoveRange(oldCategoryCritireaList);
                        //await _context.SaveChangesAsync();
                        category.Category = _context.Categories.Where(c => c.Id == id).FirstOrDefault();

                        category.Category.CategoryDocuments = new List<CategoryDocument>();
                        if (category.CategoryDocuments != null)
                        {
                            var oldCategoryDocuments = _context.CategoryDocuments.Where(a => a.CategoryId == id).ToList();
                            _context.CategoryDocuments.RemoveRange(oldCategoryDocuments);
                            await _context.SaveChangesAsync();
                            foreach (var uploadDoc in category.CategoryDocuments)
                            {
                                var categoryDocument = new CategoryDocument
                                {
                                    SupportingDocumentPath = await _fileInfo
                                        .SaveUploadFile(_hostEnv.WebRootPath, AppSetting.Instance.CategoryDocumentPath,
                                            uploadDoc).ConfigureAwait(false),
                                    DocumentName = uploadDoc.FileName

                                };
                                category.Category.CategoryDocuments.Add(categoryDocument);
                            }
                        }
                        category.Category.Name = category.Name;
                        category.Category.Description = category.Description;
                        category.Category.AwardType = category.AwardType;
                        category.Category.OpeningDate = category.OpeningDate;
                        category.Category.ClosingDate = category.ClosingDate;
                        category.Category.IdAward = category.IdAward;
                        category.Category.UpdatedDate = DateTime.Now;

                        category.Category.CategoryCriteria = ct.Category.CategoryCriteria;

                        //var cat = new Category();
                        //cat = category.Category;
                        // var newCategoryCritireaList = category.Category.CategoryCriteria.Select(a => { a.IdCategory = id; a.Category = category.Category; return a; }).ToList();
                        //category.Category.CategoryCriteria = newCategoryCritireaList;
                        _context.Update(category.Category);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Category.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            ViewData["IdAward"] = new SelectList(_context.Awards, "Id", "Description", category.IdAward);
            return View(category);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> saveCriteria(CategoryViewModel model)
        {
            try
            {
                long status = 1;
                string isSave = "1";
                var message = "";
                int counter = 1;
                int criteriaCouter = 1;
                // var qsmEntryDetail = _context.QSMEntries.FirstOrDefault(o => o.AssignedUserId == model.AssignedUserId && o.CategoryId == model.Category.Id) ;

                var qsmEntryDetail = (from q in _context.QSMEntries
                                      join c in _context.Categories on q.CategoryId equals c.Id
                                      join a in _context.Awards on c.IdAward equals a.Id
                                      join u in _context.Users on q.AssignedUserId equals u.Id
                                      where q.AssignedUserId == model.AssignedUserId && q.CategoryId == model.Category.Id
                                      select new
                                      {
                                          UserName = u.FirstName,
                                          CatName = c.Name,
                                          AwardName = a.Name,
                                          Id = q.Id,
                                          SubmittedUserId = q.SubmittedUserId,
                                          CurrentStatusId=q.CurrentStatusId 

                                      }).FirstOrDefault();


                var qsmId = qsmEntryDetail.SubmittedUserId;
                var qsmEntryId = qsmEntryDetail.Id;


                for (int i = 0; i < model.criteriaViewModel.Count; i++)
                {
                    int subCriteriaCounter = 1;

                    //foreach (var subCriteria in model.criteriaViewModel[i].subCriteriaViewModel)
                    for (int j = 0; j < model.criteriaViewModel[i].subCriteriaViewModel.Count; j++)
                    {
                        if (User.IsInRole("QualitySectionManager"))
                        {
                            var userAnswerId = _context.UserAnswers.FirstOrDefault(o => o.QsmEntryId == qsmEntryId && o.SubCriteriaId == model.criteriaViewModel[i].subCriteriaViewModel[j].Id && o.CategoryId == model.Category.Id) == null ? 0 : _context.UserAnswers.FirstOrDefault(o => o.QsmEntryId == qsmEntryId && o.SubCriteriaId == model.criteriaViewModel[i].subCriteriaViewModel[j].Id && o.CategoryId == model.Category.Id).Id;

                            if (model.criteriaViewModel[i].subCriteriaViewModel[j].Comments != null && model.criteriaViewModel[i].subCriteriaViewModel[j].Comments != "")
                            {
                                UserAnswersComments uAnswerComments = new UserAnswersComments();
                                uAnswerComments.Comments = model.criteriaViewModel[i].subCriteriaViewModel[j].Comments;
                                uAnswerComments.UserAnswerId = userAnswerId;
                                uAnswerComments.CurrentStatusId = model.IsSubmitted == true ? 4 : 3;//Qsm saved without submit
                                uAnswerComments.CreateDate = DateTime.Now;
                                await _context.AddAsync(uAnswerComments).ConfigureAwait(false);
                                await _context.SaveChangesAsync();
                                isSave = "1";
                            }
                            else
                            {

                                //message = message + counter + ". Please insert comment of sub criteria no of  " + subCriteriaCounter + " of criteria no" + criteriaCouter + "." + "<br />";
                                //isSave = "2";
                                //counter++;

                            }

                        }
                        else
                        {
                            var isAttachmentExist = _context.CriteriaDocuments.Where(d => d.QSMEntryId == qsmEntryId && d.SubCriteriaId == model.criteriaViewModel[i].subCriteriaViewModel[j].Id).ToList();
                            if (model.criteriaViewModel[i].subCriteriaViewModel[j].SubCategoryDocuments1 != null && model.criteriaViewModel[i].subCriteriaViewModel[j].SubCategoryDocuments1?.Count < 3)
                            {
                                var doc = 0;
                                foreach (var uploadDoc in model.criteriaViewModel[i].subCriteriaViewModel[j].SubCategoryDocuments1)
                                {
                                    var categoryDocument = new CriteriaDocument
                                    {
                                        SupportingDocumentPath = await _fileInfo
                                            .SaveUploadFile(_hostEnv.WebRootPath, AppSetting.Instance.CategoryDocumentPath,
                                              uploadDoc).ConfigureAwait(false),
                                        CurrentStatusId = 1,
                                        QSMEntryId = qsmEntryId,
                                        CreateDate = DateTime.Now,
                                        SubCriteriaId = model.criteriaViewModel[i].subCriteriaViewModel[j].Id,
                                        DocumentNo = doc + 1

                                    };

                                    if (isAttachmentExist.Count == 0)
                                    {
                                        await _context.AddAsync(categoryDocument).ConfigureAwait(false);

                                    }
                                    else
                                    {
                                        var result = (from ua in _context.CriteriaDocuments
                                                      where ua.QSMEntryId == qsmEntryId && ua.SubCriteriaId == model.criteriaViewModel[i].subCriteriaViewModel[j].Id && ua.DocumentNo == doc + 1
                                                      select ua).FirstOrDefault();

                                        result.SupportingDocumentPath = categoryDocument.SupportingDocumentPath;
                                        result.UpdatedDate = DateTime.Now;
                                    }


                                    await _context.SaveChangesAsync();

                                }
                            }
                            else
                            {
                                if (isAttachmentExist.Count == 0 && model.criteriaViewModel[i].subCriteriaViewModel[j].SubCategoryDocuments1 == null)
                                {
                                    isSave = "2";
                                    message = message + counter + " Please upload attachment of subcriteria" + subCriteriaCounter + " of criteria no" + criteriaCouter + "<br />";
                                    counter++;
                                }
                                if (model.criteriaViewModel[i].subCriteriaViewModel[j].SubCategoryDocuments1?.Count > 2)
                                {
                                    isSave = "2";
                                    message = message + "Only Two Files are allowed for one sub criteria" + "<br />";
                                    counter++;
                                }

                            }

                            if (string.IsNullOrEmpty(model.criteriaViewModel[i].subCriteriaViewModel[j].Answer))
                            {

                                isSave = "2";
                                message = message + counter + ". Please answer sub criteria no " + subCriteriaCounter + " of criteria no" + criteriaCouter + "." + "<br />";
                                counter++;
                            }
                            else if (string.IsNullOrEmpty(model.criteriaViewModel[i].subCriteriaViewModel[j].Comments) && (!string.IsNullOrEmpty(model.criteriaViewModel[i].subCriteriaViewModel[j].CommentsDetail)))
                            {
                                //if (!string.IsNullOrEmpty(model.criteriaViewModel[i].subCriteriaViewModel[j].CommentsDetail))
                                //{
                                    message = message + counter + ". Please insert comment of sub criteria no of  " + subCriteriaCounter + " of criteria no" + criteriaCouter + "." + "<br />";
                                    isSave = "2";
                                    counter++;
                                //}


                            }
                            else if (model.criteriaViewModel[i].subCriteriaViewModel[j].Answer !=null && model.criteriaViewModel[i].subCriteriaViewModel[j].Answer.ToString().Length > model.criteriaViewModel[i].subCriteriaViewModel[j].MaxWords)
                            {
                                isSave = "2";
                                message = message + counter + ". Marks length of  sub criteria no " + subCriteriaCounter + " of criteria no " + criteriaCouter + "can not be greater than max words." + "<br />";
                                counter++;
                            }

                            else
                            {

                                var userAnswers = new UserAnswers();
                                userAnswers.CreateDate = DateTime.Now;
                                userAnswers.QsmEntryId = qsmEntryId;
                                userAnswers.CategoryId = model.Category.Id;
                                userAnswers.CriteriaId = model.criteriaViewModel[i].Id;

                                if (model.ButtonClickStatus == "save")
                                {
                                    userAnswers.CurrentStatusId = 1;// User did not Submitted the Response;
                                }
                                else
                                {
                                    userAnswers.CurrentStatusId = 2;// User Submitted the Response;
                                }

                                //userAnswers.CurrentStatusId = 2;//2 User Submitted the Response;
                                userAnswers.SubCriteriaId = model.criteriaViewModel[i].subCriteriaViewModel[j].Id;
                                userAnswers.Answer = model.criteriaViewModel[i].subCriteriaViewModel[j].Answer;
                                var userAnswerId = _context.UserAnswers.FirstOrDefault(o => o.QsmEntryId == qsmEntryId && o.SubCriteriaId == model.criteriaViewModel[i].subCriteriaViewModel[j].Id && o.CategoryId == model.Category.Id) == null ? 0 : _context.UserAnswers.FirstOrDefault(o => o.QsmEntryId == qsmEntryId && o.SubCriteriaId == model.criteriaViewModel[i].subCriteriaViewModel[j].Id && o.CategoryId == model.Category.Id).Id;
                                if (userAnswerId != 0)
                                {

                                    var result = (from ua in _context.UserAnswers
                                                  where ua.Id == userAnswerId
                                                  select ua).SingleOrDefault();
                                    userAnswers.Id = userAnswerId;

                                    result.Answer = model.criteriaViewModel[i].subCriteriaViewModel[j].Answer;
                                    if (model.ButtonClickStatus == "save")
                                    {
                                        result.CurrentStatusId = 1;// User did not Submitted the Response;
                                    }
                                    else
                                    {
                                        result.CurrentStatusId = 2;// User Submitted the Response;
                                    }
                                    _context.SaveChanges();
                                }
                                else if (userAnswers.Answer != null)
                                {

                                    await _context.AddAsync(userAnswers).ConfigureAwait(false);
                                    await _context.SaveChangesAsync();
                                }

                                if (!string.IsNullOrEmpty(model.criteriaViewModel[i].subCriteriaViewModel[j].Comments))
                                {
                                    var uAnswerComments = new UserAnswersComments();
                                    uAnswerComments.Comments = model.criteriaViewModel[i].subCriteriaViewModel[j].Comments;
                                    uAnswerComments.UserAnswerId = userAnswers.Id;
                                    uAnswerComments.CurrentStatusId = 1;
                                    uAnswerComments.CreateDate = DateTime.Now;
                                    await _context.AddAsync(uAnswerComments).ConfigureAwait(false);
                                    await _context.SaveChangesAsync();
                                    isSave = "1";
                                }

                            }
                        }
                        subCriteriaCounter++;
                    }
                    criteriaCouter++;
                }

                if (User.IsInRole("QualitySectionManager"))
                {
                    if (model.IsSubmitted == true && isSave == "1")
                    {
                        var result = (from ua in _context.QSMEntries
                                      where ua.Id == qsmEntryId
                                      select ua).SingleOrDefault();
                        result.CurrentStatusId = 4;//Submitted From QSM;
                        _context.SaveChanges();
                        string notificationMessage = _localizer["AnswersAcceptedByQSM"];
                        notificationMessage = notificationMessage.Replace("QSMUSER", CurrentUserFullName);
                        notificationMessage = notificationMessage.Replace("CatName", qsmEntryDetail.CatName);
                        notificationMessage = notificationMessage.Replace("AwardName", qsmEntryDetail.AwardName);
                        InsertNotification(model.AssignedUserId, notificationMessage, 3);

                        var adminList = _context.UserRoles.Where(ur => ur.RoleId == 1).ToList();
                        foreach (var id in adminList)
                        {
                            string notificationMessageForAdmin = _localizer["NotificationMessageForAdmin"];
                            notificationMessageForAdmin = notificationMessage.Replace("QSMUSER", CurrentUserFullName);
                            InsertNotification(id.UserId, notificationMessage, 3);
                        }

                    }
                    if (model.IsSubmitted == false && isSave == "1")
                    {
                        var result = (from ua in _context.QSMEntries
                                      where ua.Id == qsmEntryId
                                      select ua).SingleOrDefault();
                        result.CurrentStatusId = 3;//Saved From QSM;
                        _context.SaveChanges();
                        string notificationMessage = _localizer["AnswersRejectedByQSM"];
                        notificationMessage = notificationMessage.Replace("QSMUSER", CurrentUserFullName);
                        notificationMessage = notificationMessage.Replace("CatName", qsmEntryDetail.CatName);
                        notificationMessage = notificationMessage.Replace("AwardName", qsmEntryDetail.AwardName);
                        InsertNotification(model.AssignedUserId, notificationMessage, 3);

                    }
                }
                else
                {
                    if (isSave == "1")
                    {
                        var result = (from ua in _context.QSMEntries
                                      where ua.Id == qsmEntryId
                                      select ua).FirstOrDefault();
                        result.CurrentStatusId = 2;//Submitted From EndUser;
                        if (model.ButtonClickStatus != null && model.ButtonClickStatus == "save")
                            result.CurrentStatusId = 1;//Saved From EndUser;
                        if (qsmEntryDetail.CurrentStatusId == 3)
                            result.CurrentStatusId = 5;//Re-Submitted from end user

                        _context.SaveChanges();
                        //var AssignedUserName = model.AssignedUserId + model.Category.Id;
                        if (model.ButtonClickStatus != "save")
                        {
                            string notificationMessage = _localizer["SubmissionFromEndUSer"];
                            notificationMessage = notificationMessage.Replace("USerName", qsmEntryDetail.UserName);
                            notificationMessage = notificationMessage.Replace("CatName", qsmEntryDetail.CatName);
                            notificationMessage = notificationMessage.Replace("AwardName", qsmEntryDetail.AwardName);
                            InsertNotification(qsmId, notificationMessage, 2);
                        }
                    }

                }


                if (model.ButtonClickStatus == "save")
                {
                    isSave = "2";
                    message = " Change(s) you have made are saved but not sent to QSM. To send these change(s) to QSM you need to click on submit button.";
                }

                return await FillCriteria(model.Category.Id, isSave, message.ToString(), qsmEntryId: qsmEntryId, isSubmitted: model.IsSubmitted);

            }
            catch (Exception ex)
            {
                return await FillCriteria(model.Category.Id, "2", ex.Message, qsmEntryId: 0, isSubmitted: false);
                /// throw;
            }
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Award)
                .FirstOrDefaultAsync(m => m.Id == id);
            category.Award = _context.Awards.SingleOrDefault(a => a.Id == category.IdAward);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);

        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            //await getLangStrings(PageType.MainWin).ConfigureAwait(false);
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> GetCriteriaDetails(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criteriaList = await _context.CategoryCriterias.Include(i => i.CategorySubCriteria).Include(i => i.Category).Where(m => m.Category.Id == id).ToListAsync();

            if (criteriaList == null || criteriaList.Count() == 0)
            {
                return NotFound();
            }

            return Json(criteriaList.Select(a => new { a.Id, a.IdCategory, a.Description, CategorySubCriteria = a.CategorySubCriteria.Select(b => new { b.Id, b.Description, b.Marks, b.MaxWords }) }));
        }
        public async Task<IActionResult> AllocateSector(long catId, long sectorId, bool isAllocated)
        {

            try
            {
                var isAlreadyExist = _context.CategorySectorEntries.Where(x => x.SectorId == sectorId && x.CategoryId == catId).Count();

                if (isAlreadyExist > 0)
                {
                    if (isAllocated == true)
                    {
                        var result = (from q in _context.CategorySectorEntries
                                      where q.SectorId == sectorId && q.CategoryId == catId
                                      select q).FirstOrDefault();

                        _context.CategorySectorEntries.Remove(result);
                        await _context.SaveChangesAsync();

                    }
                }

                else
                {
                    if (isAlreadyExist == 0 && isAllocated == false)
                    {
                        CategorySectorEntry categorySectorEntry = new CategorySectorEntry();
                        categorySectorEntry.CreateDate = DateTime.Now;
                        categorySectorEntry.CategoryId = catId;
                        categorySectorEntry.CurrentStatusId = 1;
                        categorySectorEntry.SectorId = sectorId;

                        await _context.AddAsync(categorySectorEntry).ConfigureAwait(false);
                        await _context.SaveChangesAsync();

                    }

                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("Opening", "");
        }
        public IActionResult AssignMember(long catId, long assignedUserId)
        {
            try
            {

                QSMEntries qSMEntries = new QSMEntries();
                qSMEntries.CreateDate = DateTime.Now;
                qSMEntries.CategoryId = catId;
                qSMEntries.CurrentStatusId = 1;
                qSMEntries.AssignedUserId = assignedUserId;
                qSMEntries.SubmittedUserId = long.Parse(CurrentUserId);
                _context.AddAsync(qSMEntries).ConfigureAwait(false);
                _context.SaveChangesAsync();
                UserAnswersViewModel userAnswersViewModel = new UserAnswersViewModel();
                userAnswersViewModel.Answer = "";
                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(userAnswersViewModel);
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                return Json("");

            }
        }


        public async Task<IActionResult> FillCriteria(long catId, string saveStatus = "0", string message = "", long assignedUserId = 0, long qsmEntryId = 0, bool isSubmitted = false)
        {
            try
            {
                //var isChecked = false;
                if (IsRolePresent("EndUser"))
                    assignedUserId = long.Parse(CurrentUserId);

                var category = await _context.Categories.FindAsync(catId).ConfigureAwait(false);
                if (category == null)
                {
                    return NotFound();
                }
                var categoryViewModel = new CategoryViewModel
                {
                    Category = category,
                    Awards = await _context.Awards.ToListAsync().ConfigureAwait(false)
                };
                categoryViewModel.CurrentStatus = 1;

                if (saveStatus == "1")
                    categoryViewModel.EnableSubmitChkBox = !isSubmitted;
                if (saveStatus == "0" || saveStatus == "2")
                    categoryViewModel.EnableSubmitChkBox = true;
                var categoryCriterias = await _context.CategoryCriterias.Where(c => c.CategoryId == category.Id).Include(c => c.CategorySubCriteria)
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (categoryCriterias != null && categoryCriterias.Count > 0)
                {
                    categoryViewModel.Category.CategoryCriteria = categoryCriterias;
                }
                QSMEntries qsmEntryData = new QSMEntries();


                if (qsmEntryId == 0)
                {
                    var data = _context.QSMEntries.FirstOrDefault(o => o.AssignedUserId == assignedUserId && o.CategoryId == catId);
                    qsmEntryId = data.Id;
                    if (data.CurrentStatusId == 4)
                        categoryViewModel.EnableSubmitChkBox = false;
                    if (data.CurrentStatusId == 1 && User.IsInRole("QualitySectionManager"))
                        categoryViewModel.EnableSubmitChkBox = false;


                }
                else
                {
                    qsmEntryData = _context.QSMEntries.FirstOrDefault(o => o.Id == qsmEntryId);
                    if (qsmEntryData.CurrentStatusId == 4)
                        categoryViewModel.EnableSubmitChkBox = false;
                    if (qsmEntryData.CurrentStatusId == 1 && User.IsInRole("QualitySectionManager"))
                        categoryViewModel.EnableSubmitChkBox = false;
                }

                List<CriteriaViewModel> cvmLst = new List<CriteriaViewModel>();

                if (categoryViewModel.Category.CategoryCriteria != null)
                {
                    foreach (var cri in categoryViewModel.Category.CategoryCriteria)
                    {
                        CriteriaViewModel cvm = new CriteriaViewModel();
                        cvm.Id = cri.Id;
                        cvm.CategoryId = cri.CategoryId;
                        cvm.Description = cri.Description;
                        List<SubCriteriaViewModel> scvmLst = new List<SubCriteriaViewModel>();
                        foreach (var sub in cri.CategorySubCriteria)
                        {
                            SubCriteriaViewModel Scvm = new SubCriteriaViewModel();
                            Scvm.Id = sub.Id;
                            Scvm.CategoryCriteriaId = sub.CategoryCriteriaId;
                            Scvm.Description = sub.Description;
                            Scvm.Marks = sub.Marks;
                            Scvm.MaxWords = sub.MaxWords;

                            var detail = _context.UserAnswers.FirstOrDefault(u => u.SubCriteriaId == sub.Id && u.QsmEntryId == qsmEntryId);
                            Scvm.Answer = detail?.Answer == null || detail.Answer == "" ? "" : detail.Answer.ToString();

                            //for (int i = 1; i <= 2; i++)
                            //{
                            //    if (i == 1)
                            //        Scvm.SubCategoryDocuments1Path = _context.CriteriaDocuments.Where(x => x.SubCriteriaId == sub.Id && x.QSMEntryId == qsmEntryId && x.DocumentNo == i).FirstOrDefault()?.SupportingDocumentPath == null ? "" : _context.CriteriaDocuments.Where(x => x.SubCriteriaId == sub.Id && x.QSMEntryId == qsmEntryId && x.DocumentNo == i).FirstOrDefault().SupportingDocumentPath;
                            //    if (i == 2)
                            //        Scvm.SubCategoryDocuments2Path = _context.CriteriaDocuments.Where(x => x.SubCriteriaId == sub.Id && x.QSMEntryId == qsmEntryId && x.DocumentNo == i).FirstOrDefault()?.SupportingDocumentPath == null ? "" : _context.CriteriaDocuments.Where(x => x.SubCriteriaId == sub.Id && x.QSMEntryId == qsmEntryId && x.DocumentNo == i).FirstOrDefault().SupportingDocumentPath;

                            //}


                            long userAnswerId = detail == null ? 0 : detail.Id; //_context.UserAnswers.FirstOrDefault(u => u.SubCriteriaId == sub.Id && u.QsmEntryId == qsmEntryId) == null ? 0 : _context.UserAnswers.FirstOrDefault(u => u.SubCriteriaId == sub.Id && u.QsmEntryId == qsmEntryId).Id;
                            var userAnswersComments = from u in _context.UserAnswersComments
                                                      where u.UserAnswerId == userAnswerId
                                                      orderby u.Id descending
                                                      select u;



                            foreach (var data in userAnswersComments)
                            {
                                Scvm.CommentsDetail = Scvm.CommentsDetail + " " + data.Comments + "\n";
                                if (categoryViewModel.CurrentStatus != 3)
                                    categoryViewModel.CurrentStatus = data.CurrentStatusId;

                            }


                            scvmLst.Add(Scvm);
                        }
                        cvm.subCriteriaViewModel = scvmLst;
                        cvmLst.Add(cvm);
                    }
                }
                else
                {
                    message = "No Criteria Exists";
                    saveStatus = "2";
                    categoryViewModel.EnableSubmitChkBox = false;

                }


                categoryViewModel.criteriaViewModel = cvmLst;
                categoryViewModel.SaveStatus = saveStatus;
                categoryViewModel.Message = message;
                categoryViewModel.AssignedUserId = assignedUserId;
                categoryViewModel.QsmEntryId = qsmEntryId;

                return View("~/Views/Categories/Criteria.cshtml", categoryViewModel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public IActionResult FillSubmission(long catId)
        {
            try
            {

                var auditorAssignModel = new List<AuditorAssignModel>();
                var result = (from qSE in _context.QSMEntries
                              join c in _context.Categories
                              on qSE.CategoryId equals c.Id
                              join u in _context.Users
                              on qSE.AssignedUserId equals u.Id
                              join a in _context.Awards
                              on c.IdAward equals a.Id
                              where qSE.CurrentStatusId == 4 && c.Id == catId
                              select new
                              {
                                  userName = u.FirstName,
                                  catId = c.Id,
                                  qsmEntryId = qSE.Id,
                                  SubmittedAr = qSE.UpdatedDate,
                                  catName = c.Name,
                                  Award = a.Name,
                                  IsSubmitted = _context.CategoryTeamEntries.Where(cTE => cTE.QsmEntryId == qSE.Id).FirstOrDefault() == null ? false : true,
                                  QSMUserId = qSE.SubmittedUserId

                              }).ToList();
                if (result?.Count() > 0)
                {
                    foreach (var data in result)
                    {
                        auditorAssignModel.Add(new AuditorAssignModel { CatId = data.catId, QsmEntryId = data.qsmEntryId, UserName = data.userName, SubmittedAt = data.SubmittedAr, CatName = data.catName, IsSubmitted = data.IsSubmitted, AwardName = data.Award, QsmUserId = data.QSMUserId });

                    }
                }
                return View("~/Views/AssignAuditor/AssignAuditor.cshtml", auditorAssignModel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        //public IActionResult ViewQsmAssignedUsers(long catId)
        //{
        //    var qsmAssignViewModel = new List<QsmAssignViewModel>();

        //    if (IsRolePresent("QualitySectionManager"))
        //    {
        //        var result = (from qs in _context.QsmSector
        //                      join cse in _context.CategorySectorEntries
        //                          on qs.SectorId equals cse.SectorId
        //                      join c in _context.Categories
        //                      on cse.CategoryId equals c.Id
        //                      join a in _context.Awards
        //                      on c.IdAward equals a.Id
        //                      join u in _context.Users
        //                      on qs.UserID equals u.Id
        //                      where qs.UserID == long.Parse(CurrentUserId)
        //                      && qs.RoleId == 2
        //                      select new
        //                      {
        //                          catId = c.Id,
        //                          catName = c.Name,
        //                          awardName = a.Name,
        //                          type = c.AwardType
        //                          //UserName =u.FirstName +" "+ (u.LastName  == null?"": u.LastName)
        //                      }).ToList();
        //        if (result.Count > 0)
        //        {
        //            foreach (var data in result)
        //            {
        //                qsmAssignViewModel.Add(new QsmAssignViewModel { CatId = data.catId, Name = data.catName, AwardName = data.awardName, AwardType = 1 });
        //            }

        //        }
        //    }
        //    ViewBag.Users = _context.Users.ToList();

        //    return View("~/Views/Categories/QsmAssign.cshtml", qsmAssignViewModel);

        //}
        private bool CategoryExists(long id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
        [HttpGet]
        public ActionResult LoadSectorData(long catId)
        {
            var categorySectorEntries = _context.CategorySectorEntries.Where(cse => cse.CategoryId == catId).ToList();
            var sectors = (from s in _context.Sectors
                           select new
                           {
                               Id = s.Id,
                               NameEn = s.NameEn,
                               NameAr = s.NameAr,
                               IsSubmitted = false,
                               IsQsmAssigned = false
                           }).ToList();

            List<CategorySectorEntriesViewModel> result = new List<CategorySectorEntriesViewModel>();

            for (int i = 0; i < sectors.Count; i++)
            {
                var isSubmitted = categorySectorEntries.Any(cse => cse.SectorId == sectors[i].Id);
                var IsQsmAssigned = (from q in _context.QSMEntries
                                     join u in _context.Users on q.AssignedUserId equals u.Id
                                     join e in _context.Employee on u.IdEmployee equals e.Id
                                     where q.CategoryId == catId && e.SectorId == sectors[i].Id
                                     select q
                                    ).FirstOrDefault();
                result.Add(new CategorySectorEntriesViewModel { Id = sectors[i].Id, NameEn = sectors[i].NameEn, NameAr = sectors[i].NameAr, IsSubmitted = isSubmitted, IsQsmAssigned = IsQsmAssigned == null ? false : true });
            }
            return Json(new { result = result });
        }

        public FileResult DownloadCategoryFiles(long catId)
        {
            var webRoot = _hostEnv.WebRootPath;
            var fileName = "myZip.zip";
            var tempOutPut = webRoot + "/temp/" + fileName;

            using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutPut)))
            {
                zipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];
                var ImageList = new List<string>();

                var categories = _context.CategoryDocuments.Where(c => c.Category.Id == catId).ToList();
                foreach (var aw in categories)
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

        public FileResult DownloadCategoriesFiles(long id)
        {
            var webRoot = _hostEnv.WebRootPath;
            var fileName = "myZip.zip";
            var tempOutPut = webRoot + "/temp/" + fileName;

            using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutPut)))
            {
                zipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];
                var ImageList = new List<string>();

                var awards = _context.CategoryDocuments.Where(c => c.Id == id).ToList();
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

    }
}
