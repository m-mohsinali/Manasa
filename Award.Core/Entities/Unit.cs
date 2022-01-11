using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Entities
{
    public class Unit : BaseEntity
    {
        //public int Id { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        //public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
