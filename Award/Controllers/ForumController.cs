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
using Award.Web.Models;

namespace Award.Web.Controllers
{
    [Authorize]

    public class ForumController : BaseController
    {
        private readonly AwardDbContext _context;
        private readonly IFileInfo _fileInfo;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly ILdapAuthenticationService _ldapAuthenticationService;
        public ForumController(AwardDbContext context, IFileInfo fileInfo, IWebHostEnvironment hostEnv, ILdapAuthenticationService ldapAuthenticationService) : base(context)
        {
            _context = context;
            _fileInfo = fileInfo;
            _hostEnv = hostEnv;
            _ldapAuthenticationService = ldapAuthenticationService;
        }
        public IActionResult Index()
        {
            var forumCategoryViewModel = new List<ForumCategoryViewModel>();

            var  categories =  _context.Categories.ToList();
            categories.ForEach(c => c.Award = _context.Awards.SingleOrDefault(a => a.Id == c.IdAward));

            foreach(var cat in categories)
            {
                ForumCategoryViewModel fr = new ForumCategoryViewModel()
                {
                    CategoryId = cat.Id,
                    CategoryName = cat.Name,
                    AwardName =cat.Award.Name,
                    AwardType = cat.AwardType,
                    TotalTopics = (_context.Forum.Where(c=>c.Category.Id == cat.Id).Count())
                };
                forumCategoryViewModel.Add(fr);
            }

            return View(forumCategoryViewModel);
        }

        public IActionResult ForumTopics(long CategoryId)
        {
            List<ForumTopicsViewModel> forumTopicsViewModel = GetForumTopics(CategoryId);

            return View(forumTopicsViewModel);
        }

        [HttpPost]
        public IActionResult CreateForumTopics(long CategoryId, string ForumText)
        {
            if (ForumText.Trim().Length > 0)
            {
                InsertForumTopic(CategoryId, ForumText);
            }

            List<ForumTopicsViewModel> forumTopicsViewModel = GetForumTopics(CategoryId);
            return View("ForumTopics", forumTopicsViewModel);
        }

        public IActionResult ForumTopicsReply(long ForumId)
        {
            List<ForumTopicsReplyViewModel> forumTopicsViewModel = GetForumTopicsReply(ForumId);

            return View(forumTopicsViewModel);
        }

        public IActionResult ForumTopicsReplyDelete(long ForumId,long ReplyId)
        {
            var cat = _context.ForumDetails.Where(d => d.Id == ReplyId).FirstOrDefault();
            _context.Remove(cat);
            _context.SaveChanges();


            List<ForumTopicsReplyViewModel> forumTopicsViewModel = GetForumTopicsReply(ForumId);

            return View("ForumTopicsReply",forumTopicsViewModel);

            //return RedirectToRoute("ForumTopicsReply", new { ForumId = ForumId });
        }

        [HttpPost]
        public IActionResult CreateForumTopicsReply(long ForumId, string ForumReply)
        {
            if (ForumReply.Trim().Length > 0)
            {
                InsertForumTopicReply(ForumId, ForumReply);
            }

            List<ForumTopicsReplyViewModel> forumTopicsViewModel = GetForumTopicsReply(ForumId);
           // return RedirectToRoute("ForumTopicsReply", new { ForumId = ForumId });
             return View("ForumTopicsReply", forumTopicsViewModel);
        }

        private List<ForumTopicsViewModel> GetForumTopics(long CategoryId)
        {
            var cat = _context.Categories.Where(d => d.Id == CategoryId).ToList();
            var forumTopicsViewModel = (from c in _context.Categories
                                        join forum in _context.Forum on c.Id equals forum.Category.Id
                                        join usr in _context.Users on forum.User.Id equals usr.Id
                                        join Awd in _context.Awards on c.IdAward equals Awd.Id
                                        where c.Id == CategoryId
                                        select new ForumTopicsViewModel()
                                        {
                                            ForumId = forum.Id,
                                            Categoryid = c.Id,
                                            CategoryName = c.Name,
                                            UserId = usr.Id,
                                            UserName = CurrentLanguage == "en" ? usr.FirstName : usr.LastName,
                                            ForumTopic = forum.ForumText,
                                            CreatedDate = forum.CreateDate,
                                            TotalReply = (_context.ForumDetails.Where(d=> d.ForumId == forum.Id).Count())
                                        }
                               ).ToList();
            ViewData["CategoryId"] = CategoryId;
            ViewData["CategoryName"] = cat.Count() > 0 ? cat.FirstOrDefault().Name : "";
            return forumTopicsViewModel;
        }

        private List<ForumTopicsReplyViewModel> GetForumTopicsReply(long ForumId)
        {
            var frm = _context.Forum.Where(d => d.Id == ForumId).ToList();
            var forumTopicsViewModel = (from c in _context.Categories
                                        join forum in _context.Forum on c.Id equals forum.Category.Id
                                        join forumReply in _context.ForumDetails on forum.Id equals forumReply.Forum.Id
                                        join usr in _context.Users on forumReply.User.Id equals usr.Id
                                        join Awd in _context.Awards on c.IdAward equals Awd.Id
                                        where forumReply.ForumId == ForumId
                                        select new ForumTopicsReplyViewModel()
                                        {
                                            ReplyId = forumReply.Id,
                                            ForumId = forumReply.Forum.Id,
                                            Categoryid = c.Id,
                                            CategoryName = c.Name,
                                            UserId = usr.Id,
                                            UserName = CurrentLanguage == "en" ? usr.FirstName : usr.LastName,
                                            ForumTopic = forum.ForumText,
                                            CreatedDate = forumReply.CreateDate,
                                            ForumTopicReply = forumReply.ForumDetailText,
                                        }
                               ).ToList();
            ViewData["ForumId"] = ForumId;
            ViewData["ForumText"] = frm.Count() > 0 ? frm.FirstOrDefault().ForumText : "";
            ViewData["CurrenrUser"] = CurrentUserId;
            return forumTopicsViewModel;
        }



        private void InsertForumTopic(long CategoryId, string ForumText)
        {
            Forum frm = new Forum();

            frm.CategoryId = CategoryId;
            frm.ForumText = ForumText;
            frm.UserId = long.Parse(CurrentUserId);
            frm.CreateDate = DateTime.Now;

            _context.Add(frm);
            _context.SaveChanges();
        }

        private void InsertForumTopicReply(long ForumId, string ForumReply)
        {
            ForumDetails frm = new ForumDetails();

            frm.ForumId = ForumId;
            frm.ForumDetailText = ForumReply;
            frm.UserId = long.Parse(CurrentUserId);
            frm.CreateDate = DateTime.Now;

            _context.Add(frm);
            _context.SaveChanges();
        }
    }
}