using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Award.Web.Common.Utils
{
    public static class Do
    {
        public static string ImageName(this String str)
        {
            List<string> newstr = str.Split('\\').ToList();
            return newstr.LastOrDefault();
        }
    }
}
