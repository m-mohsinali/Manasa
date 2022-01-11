using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class WinnerViewModel
    {
            public Winner winner { get; set; }

           public IList<Awards> Awards { get; set; }
            public IList<Sector> Sectors { get; set; }
            public IList<Employee> Employees { get; set; }
            public IList<Category> Categories { get; set; }

        public bool IsAnnounced { get; set; }
        public bool ShowWinnerImage { get; set; }
        public DateTime? AnounceDate { get; set; }
        public long IdAward { get; set; }
        public long IdCategory { get; set; }
        public long? IdEmployee { get; set; }
        public long? IdSector { get; set; }
        public virtual Category Category { get; set; }
        public virtual Awards Award { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Sector Sector { get; set; }

    }
}
