using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class DashboardViewModel
    {
        public long TotalAwards { get; set; }
        public long TotalCategories { get; set; }
        public long TotalTeams { get; set; }
        public long TotalDepartments { get; set; }
        public long TotalSectors { get; set; }
        public long TotalEmpShowIntrest { get; set; }
        public long ParticipatedSectors { get; set; }
        public long TotalSubmissionFromQSM { get; set; }
        public long TotalSubmissionFromAuditManager { get; set; }
        public long TotalSubmissionFromJury { get; set; }
    }
}
