using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Award.Core.Common;
using Award.Core.Interfaces;
using Award.Infrastructure.Data;
using Award.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Award.Core.Common.Constants;

namespace Award.Web.Controllers
{

    [Authorize]

    public class ChartsController : Controller
    {
        private readonly AwardDbContext _context;
        private readonly ICategoryServices _categoryServices;


        public ChartsController(AwardDbContext context, ICategoryServices categoryServices)
        {
            _context = context;
            _categoryServices = categoryServices;
            
        }
        public IActionResult Index()
        {
            return View();
        }
        private Random rnd = new Random();

        [HttpPost]
        public ActionResult NewChart(long catIdTest)
        {
            var constants = new Constants();
            var dtColors = constants.GetColors();
           // var dt = _categoryServices.GetCategoryCount();
            var dt = _categoryServices.GetSectorsCount();

            


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Color"] = dtColors.Rows[i][0];
            }


            List<object> iData = new List<object>();
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            //return Json(iData, JsonRequestBehavior.AllowGet);
            return Json(new { chData = iData });

        }

        [HttpPost]
        public ActionResult AwardsCharts()
        {
            var constants = new Constants();
            var dtColors = constants.GetColors();
            var dt = _categoryServices.GetCategoryCount();
          //  var dt = _categoryServices.GetSectorsCount();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Color"] = dtColors.Rows[i][0];
            }
            List<object> iData = new List<object>();
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            return Json(new { chData = iData });
        }
        



        public IActionResult PieChart()
        {
            ViewBag.Sectors =  _context.Sectors.ToList();
            string tempMobile = string.Empty;
            string tempProduct = string.Empty;
           // _ICharts.ProductWiseSales(out tempMobile, out tempProduct);
            ViewBag.MobileCount_List = "234";

            return View();
        }

    }
}