using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class ShowWinnersViewModel
    {
        public long Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImage { get; set; }
        public string Award { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public long UserId { get; set; }
        public bool ShowImage { get; set; }
    }
}
