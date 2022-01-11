using System;


namespace Award.Core.Entities
{
    public class CategoryEmployee : BaseEntity
    {
        //public long Id { get; set; }
        public long IdEmployee { get; set; }
        public long IdCategory { get; set; }
        public short NotifiedTries { get; set; }
        public DateTime AssignedAt { get; set; }

        public virtual Category IdCategoryNavigation { get; set; }
        public virtual Employee IdEmployeeNavigation { get; set; }
    }
}
