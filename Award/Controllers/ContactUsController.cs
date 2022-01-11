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
using System.Linq;
using System.Collections.Generic;
using System;

namespace Award.Web.Controllers
{
    public class ContactUsController : BaseController
    {
        private readonly AwardDbContext _context;
        private readonly IFileInfo _fileInfo;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly ILdapAuthenticationService _ldapAuthenticationService;
        public ContactUsController(AwardDbContext context, IFileInfo fileInfo, IWebHostEnvironment hostEnv, ILdapAuthenticationService ldapAuthenticationService) : base(context)
        {
            _context = context;
            _fileInfo = fileInfo;
            _hostEnv = hostEnv;
            _ldapAuthenticationService = ldapAuthenticationService;
        }
        public IActionResult Index()
        {
            ViewData["Lang"] = CurrentLanguage;
            List<Contact> contacts = _context.Contacts.OrderBy(d => d.Order).ToList(); 
            return View(contacts);
        }
        public IActionResult Show()
        {
            ViewData["Lang"] = CurrentLanguage;
            List<Contact> contacts = _context.Contacts.OrderBy(d => d.Order).ToList();
            return View(contacts);
        }
        public IActionResult Create(long ContactId=0)
        {
            ViewData["Lang"] = CurrentLanguage;
            if (ContactId != 0)
            {
                Contact contacts = _context.Contacts.Where(d=>d.Id == ContactId).FirstOrDefault();
                return View(contacts);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contact model)
        {
            ViewData["Lang"] = CurrentLanguage;
            model.CreateDate = DateTime.Now;
            if(model.Id !=0)
            {
                _context.Update(model);
            }
            else
            {
                _context.Add(model);
            }
           
            _context.SaveChanges();

            return RedirectToAction("show", new { ContactId =model.Id });
        }

        
        public IActionResult Delete(long ContactId = 0)
        {
            ViewData["Lang"] = CurrentLanguage;
            Contact contacts = _context.Contacts.Where(d => d.Id == ContactId).FirstOrDefault();
           
            _context.Remove(contacts);
            _context.SaveChanges();

            return RedirectToAction("show", new { ContactId = 0 });
        }

    }
}