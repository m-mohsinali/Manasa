using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Entities
{
    public class QualitySectorManager : BaseEntity
    {
        public long SectorId { get; set; }
        public long UserId { get; set; }

        public virtual Sector Sector { get; set; }
        public virtual User User { get; set; }
    }
}
