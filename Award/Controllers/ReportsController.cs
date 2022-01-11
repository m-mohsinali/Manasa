using Award.Core.Entities;
using Award.Core.Interfaces;
using Award.Infrastructure.Data;
using Award.Web.Common.Utils;
using Award.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly AwardDbContext _context;
        private readonly IFileInfo _fileInfo;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IStringLocalizer<NotificationController> _localizer;
        private readonly IManasaEmployeeRepository _manasaEmployeeRepository;

        public ReportsController(AwardDbContext context, IFileInfo fileInfo, IHostingEnvironment hostEnv, IStringLocalizer<NotificationController> localizer, IManasaEmployeeRepository manasaEmployeeRepository) : base(context)
        {
            _context = context;
            _fileInfo = fileInfo;
            _hostEnv = hostEnv;
            _localizer = localizer;
            _manasaEmployeeRepository = manasaEmployeeRepository;

        }
        public IActionResult Index()
        {
            var data = _manasaEmployeeRepository.GetReportGenericData();
            return View("~/Views/Reports/GenericReportView.cshtml", _manasaEmployeeRepository.GetReportGenericData());
        }

        public ActionResult print()
        {

            var data = _manasaEmployeeRepository.GetReportGenericData();
           // data.MediaType = "Print";
            var report = new ViewAsPdf("GenericReportView", data);
            return report;
        }
    }
}
