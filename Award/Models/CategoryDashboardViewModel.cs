using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class CategoryDashboardViewModel
    {
        public long CategoryId { get; set; }
        public long AwardId { get; set; }
        public string CategoryName { get; set; }
        public string AwardName { get; set; }
        public AwardType AwardType { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]

        public DateTime OpeningDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]

        public DateTime ClosingDate { get; set; }
        public long TotalExpressedIntrest { get; set; }
        public long TotalAssignedByQsm { get; set; }
        public long TotalSubmissionByQsm { get; set; }
        public long TotalSubmissionByAuditManager { get; set; }
    }
    public class ParticipatedSectorDashboardViewModel
    {
        public string SectorName { get; set; }
        public long CategoryCount { get; set; }
    }
    public class QSMSubmissionViewModel
    {
        public string CategoryName { get; set; }
        public string AwardName { get; set; }
        public string AssignedUser { get; set; }
        public string QSM { get; set; }
        public string SectorName { get; set; }

    }

}
