using System;

namespace Award.Core.Entities
{
    public class CategorySector : BaseEntity
    {
        //public long Id { get; set; }
        public int IdSector { get; set; }
        public long IdCategory { get; set; }
        public bool? IsNotified { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime? AssignedAt { get; set; }

        public virtual Category Category { get; set; }
        public virtual Sector Sector { get; set; }
    }
}
