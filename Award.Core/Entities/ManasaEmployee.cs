using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Entities
{
    public class ManasaEmployee
    {
        public int ID { get; set; }
        public int? GRP { get; set; }
        public string NameEN { get; set; }
        public string NameAR { get; set; }
        public string RankAR { get; set; }
        public string RankEN { get; set; }
        public string JobTitleEN { get; set; }
        public string JobTitleAR { get; set; }
        public string ClassEN { get; set; }
        public string ClassAR { get; set; }
        public string SectorNameAR { get; set; }
        public string SectorNameEN { get; set; }
        public string DeptNameAR { get; set; }
        public string DeptNameEN { get; set; }
        public string SectionNameAR { get; set; }
        public string SectionNameEN { get; set; }
        public string UnitNameEN { get; set; }
        public string UnitNameAR { get; set; }
        public string BranchNameEN { get; set; }
        public string BranchNameAR { get; set; }
        public string EmployeePhotoURL { get; set; }
        public DateTime? LastUpdaeDate { get; set; }
        public string UserDomain { get; set; }
        public string SexEN { get; set; }
        public string SexAR { get; set; }
    }

}
