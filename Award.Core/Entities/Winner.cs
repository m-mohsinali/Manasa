using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Award.Core.Entities
{
     public class Winner : BaseEntity
    {
        public bool IsAnnounced { get; set; }
        public bool ShowWinnerImage { get; set; }
        [DataType(DataType.Date)]
        public DateTime? AnounceDate { get; set; }
        public long AwardId { get; set; }
        public long CategoryId { get; set; }
        public long? EmployeeId { get; set; }
        public long? SectorId { get; set; }
        public long? JuryId { get; set; }

        //public long IdAward { get; set; }
        //public long IdCategory { get; set; }
        //public long? IdEmployee { get; set; }
        //public long? IdSector { get; set; }

        public virtual Category Category { get; set; }
        public virtual Awards Award { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Sector Sector { get; set; }
    }
}
