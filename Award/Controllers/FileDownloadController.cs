using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Award.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Award.Web.Controllers
{
    public class FileDownloadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public class FileDownloads
        {
            //public List<Award.Web.Models.FileInfo> GetFile()
            //{
            //    List<Award.Web.Models.FileInfo> listFiles = new List<Award.Web.Models.FileInfo>();
            //    string fileSavePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images");
            //    DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
            //    int i = 0;
            //    foreach (var item in dirInfo.GetFiles())
            //    {
            //        listFiles.Add(new Award.Web.Models.FileInfo()
            //        {
            //            FileId = i + 1,
            //            FileName = item.Name,
            //            FilePath = dirInfo.FullName + @ "\" + item.Name  
            //        });
            //        i = i + 1;
            //    }
            //    return listFiles;
            //}
        }
    }
}