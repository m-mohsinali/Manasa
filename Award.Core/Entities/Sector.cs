using System;
using System.Collections.Generic;


namespace Award.Core.Entities
{
    public class Sector : BaseEntity
    {
        //public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        //public DateTime? CreatedAt { get; set; }

        public virtual ICollection<CategorySector> CategorySector { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<QualitySectorManager> QualitySectorManagers { get; set; }
    }
}
