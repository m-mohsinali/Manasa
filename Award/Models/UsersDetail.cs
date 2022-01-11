using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class UsersDetail
    {
        public long  EmployeeId { get; set; }
        public string  EmployeeNameEn { get; set; }
        public string EmployeeNameAr { get; set; }
        public string Email { get; set; }
        public string Sector { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public string Section { get; set; }
        public string Branch { get; set; }
        public bool IsAssigned { get; set; }
        public long UserId { get; set; }
        public long qsmEntryStatus { get; set; }
        public string Rank { get; set; }
        public string AwardName { get; set; }
        public string CatName { get; set; }
        public string Gender { get; set; }
        public string Job { get; set; }
        public string Name { get; set; }
        public string EmployeeImageURL { get; set; }
        public string CurrentStatus { get; set; }
        public string JobEn { get; set; }
        public string JobAr { get; set; }
        public string RankEn { get; set; }
        public string RankAr { get; set; }
        public string Group { get; set; }
    }
}
