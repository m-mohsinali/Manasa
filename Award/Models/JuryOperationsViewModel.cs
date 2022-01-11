using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class JuryOperationsViewModel
    {
        public string AwardTittle { get; set; }
        public string Sector { get; set; }
        public string ParticipatedBy { get; set; }
        public long  ScoredMarks { get; set; }
        public string EvaluatedBy { get; set; }
        public string Position { get; set; }
        public string CategoryName { get; set; }
        public bool IsAnnounced { get; set; }
        public bool ShowWinnerImage { get; set; }
        public string  AnounceDate { get; set; }

        public long Id { get; set; }
        public long SectorId { get; set; }
        public long CatId { get; set; }
        public long AwardId { get; set; }
        public long EmpId { get; set; }


    }
}
