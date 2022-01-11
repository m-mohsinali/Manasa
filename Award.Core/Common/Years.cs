using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Award.Core.Common
{
    public class Years
    {
        public IEnumerable<SelectListItem> Year()
        {
            return new SelectListItem[]
            {
                  new SelectListItem() { Text = "2015", Value = "2015" },
                  new SelectListItem() { Text = "2016", Value = "2016" },
                  new SelectListItem() { Text = "2017", Value = "2017" },
                  new SelectListItem() { Text = "2018", Value = "2018" },
                  new SelectListItem() { Text = "2019", Value = "2019" },
                  new SelectListItem() { Text = "2020", Value = "2020" },
                  new SelectListItem() { Text = "2021", Value = "2021" }
            };
        }
    }
}
