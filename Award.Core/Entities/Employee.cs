using System;
using System.Collections.Generic;

namespace Award.Core.Entities
{
    public class Employee : BaseEntity
    {
        //public long Id { get; set; }
        public long  SectorId { get; set; }

        public long? DepartmentId { get; set; }

        public long? SectionId { get; set; }

        public long? BranchId { get; set; }

        public long? UnitId { get; set; }

        public string  NameAr { get; set; }

        public string NameEn { get; set; }

        //public int? Gender { get; set; }
        public string SexEn { get; set; }

        public string SexAr { get; set; }
        public string Grp { get; set; }
        public string UserDomain { get; set; }

        public string RankAr { get; set; }

        public string RankEn { get; set; }

        public string JobAr { get; set; }
        public string JobEn { get; set; }

        public string ClassAr { get; set; }

        public string ClassEn { get; set; }

        public string EmployeePhotoUrl { get; set; }

        public DateTime? LastUpdateAt { get; set; }

        public virtual Sector Sector { get; set; }

        public virtual Department Department { get; set; }

        public virtual Section Section { get; set; }

        public Branch Branch { get; set; }

        public Unit Unit { get; set; }

        //public virtual ICollection<AppUser> AppUsers { get; set; } 
       // public virtual ICollection<CategoryEmployee> CategoryEmployee { get; set; }
        public virtual ICollection<EmployeeTeams> EmployeeTeams { get; set; }
    }
}
