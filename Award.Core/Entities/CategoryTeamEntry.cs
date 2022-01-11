using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Entities
{
   public class CategoryTeamEntry:BaseEntity
    {
        public long TeamId { get; set; }
        public long QsmEntryId { get; set; }
        public long CurrentStatusId { get; set; }
    }
}
