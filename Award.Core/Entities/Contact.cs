using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Award.Core.Entities
{
    public class Contact : BaseEntity
    {
        public string EmployeeNumber { get; set; }
        public string EmployeeNameEn { get; set; }
        public string EmployeeNameAr { get; set; }
        public string RankEn { get; set; }
        public string RankAr { get; set; }
        public string JobEn { get; set; }
        public string JobAr { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
   
}
