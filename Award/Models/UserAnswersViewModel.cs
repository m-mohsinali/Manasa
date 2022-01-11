using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class UserAnswersViewModel
    {
        public long  QsmEntryId { get; set; }
        public string  AwardName { get; set; }
        public string CategoryName { get; set; }
        public string CriteriaDesc { get; set; }
        public string SubCriteriaDesc { get; set; }
        public long Marks { get; set; }
        public long MaxWords { get; set; }
        public long RoleId { get; set; }
        public string Answer { get; set; }
        public string Comments { get; set; }
        public string AssignedTo { get; set; }
        public long UserAnswerId { get; set; }
        public long CriteriaId { get; set; }
        public long SubCriteriaId { get; set; }





    }
}
