using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Entities
{
   public  class ReportGenericData
    {
        public long AwardId { get; set; }
        public string AwardName { get; set; }
        public string AwardDescription { get; set; }
        public string AwardOpeningDate { get; set; }
        public string AwardClosingDate { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescirption { get; set; }
        public string OpeningDate { get; set; }
        public string ClosingDate { get; set; }
        public string DescEn { get; set; }
        public string DescAr { get; set; }
        public string CategoryCriteraDescription { get; set; }
        public string CatSubCriteriaDescription { get; set; }
        public string Answer { get; set; }
        public string AssignedUserEnglish { get; set; }
        public string AssignedUserArabic { get; set; }
        public string QSMEnglish { get; set; }
        public string QSMEnglishArabic { get; set; }
        public string MediaType = "Print";

    }
}
