using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Award.Core.Entities;
using Award.Infrastructure.Data;
using Award.Web.Common.Utils;
using Award.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Award.Web.Controllers
{
    [Authorize]

    public class UserAnswersController : BaseController
    {
        private readonly AwardDbContext _context;

        private readonly IFileInfo _fileInfo;
        private readonly IHostingEnvironment _hostEnv;
        public UserAnswersController(AwardDbContext context, IFileInfo fileInfo, IHostingEnvironment hostEnv) : base(context)
        {
            _context = context;
            _fileInfo = fileInfo;
            _hostEnv = hostEnv;
        }
        public IActionResult Index()
        {
            long roleId = 1;
            if (IsRolePresent("QualitySectionManager"))
                 roleId = 2;
            if (IsRolePresent("EndUser"))
                roleId = 3;
            ViewBag.Role = roleId;


            var userAnswersViewModel = new List<UserAnswersViewModel>();

            var result = (from c in _context.Categories
                          join cc in _context.CategoryCriterias
                              on c.Id equals cc.CategoryId
                          join a in _context.Awards
                          on c.IdAward equals a.Id
                          join csc in _context.CategorySubCriterias
                          on cc.Id equals csc.CategoryCriteriaId
                          join qe in _context.QSMEntries
                          on c.Id equals qe.CategoryId
                          join u in _context.Users 
                          on qe.AssignedUserId equals u.Id 
                          where (qe.SubmittedUserId ==long.Parse( CurrentUserId)  ||  qe.AssignedUserId == long.Parse(CurrentUserId))
                              select new
                              {
                                  qsmEntryId = qe.Id,
                                  award = a.Name,
                                  category = c.Name,
                                  criteriaDesc = cc.Description,
                                  subCriteriaDesc = csc.Description,
                                  marks = csc.Marks,
                                  maxWords = csc.MaxWords,
                                  type = c.AwardType,
                                  submittedUserId= qe.SubmittedUserId,
                                  AssignedTo= u.FirstName + " " + (u.LastName == null ? "" : u.LastName),
                                  CriteriaId=cc.Id,
                                  SubCriteriaId=csc.Id

                              }).ToList();


            if (result.Count > 0)
                {
                    foreach (var data in result)
                    {
                        userAnswersViewModel.Add(new UserAnswersViewModel { QsmEntryId = data.qsmEntryId, AwardName = data.award, CategoryName = data.category, CriteriaDesc = data.criteriaDesc, SubCriteriaDesc = data.subCriteriaDesc, Marks = data.marks, MaxWords = data.maxWords,AssignedTo =data.AssignedTo,CriteriaId = data.CriteriaId, SubCriteriaId =data.SubCriteriaId });
                    }
                    ViewBag.Users = _context.Users.ToList();

                    return View("~/Views/UserAnswers/Index.cshtml", userAnswersViewModel);
                }
            

            return View();


        }
        public IActionResult SaveAnswers(long qsmId, string answer, string comment, int status,int criteriaId,int subCriteriaId)
        {
            try
            {

                UserAnswers userAnswers = new UserAnswers();
                userAnswers.CreateDate = DateTime.Now;
                userAnswers.QsmEntryId = qsmId;
                userAnswers.CriteriaId = criteriaId;
                userAnswers.SubCriteriaId = subCriteriaId;
                userAnswers.Answer = answer;
                userAnswers.CurrentStatusId = status;
                _context.AddAsync(userAnswers).ConfigureAwait(false);
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

        public IActionResult SaveAnswersComments(long qsmId, string comment, int status)
        {
            try
            {
                var uA = (from ua in _context.UserAnswers
                          where ua.QsmEntryId == qsmId
                          orderby ua.Id descending
                          select new
                          {
                              userAnswerId = ua.Id
                          }).FirstOrDefault();


                var userAnswers = new UserAnswersComments();
                userAnswers.CreateDate = DateTime.Now;
                userAnswers.UserAnswerId = uA.userAnswerId ;
                userAnswers.Comments = comment;
                userAnswers.CurrentStatusId = status;
                _context.AddAsync(userAnswers).ConfigureAwait(false);
                _context.SaveChangesAsync();
                 UserAnswersViewModel userAnswersViewModel = new UserAnswersViewModel();
                userAnswersViewModel.Answer = "";
                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(userAnswersViewModel);
                return Json(jsonData);
               
            }
            catch (Exception ex)
            {
                // categoryViewModel.Awards = await _context.Awards.ToListAsync().ConfigureAwait(false);
            }

            //return View(categoryViewModel);
            return View();
        }
        public IActionResult GetanswersDetails(int? qsmId,int criteriaId, int subCriteriaId)
        {


            var result =
    (from m in _context.QSMEntries
     join ma in _context.UserAnswers on m.Id equals ma.QsmEntryId into mma
     from ma in mma.DefaultIfEmpty()
     join p in _context.UserAnswersComments on (ma == null ? 0 : ma.Id) equals p.UserAnswerId into pma
     from p in pma.DefaultIfEmpty()
     where m.Id == qsmId && ma.CriteriaId ==criteriaId && ma.SubCriteriaId ==subCriteriaId 
     // orderby m.Name
     select new
     {
         Answer = ma.Answer !=null ?ma.Answer :"",
         Comments = p != null ? p.Comments : "",
         UserAnswerId=ma.Id.ToString() ==null ?0:ma.Id 

     });

          List <UserAnswersViewModel> userAnswersViewModel = new List<UserAnswersViewModel>();

            foreach (var data in result)
            {
                userAnswersViewModel.Add(new UserAnswersViewModel { Answer  =data.Answer ,Comments =data.Comments ==null ?"":data.Comments, UserAnswerId =data.UserAnswerId });
            }
            var jsonData =  Newtonsoft.Json.JsonConvert.SerializeObject(userAnswersViewModel);
            return Json(jsonData);
        }


    }
}