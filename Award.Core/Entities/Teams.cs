using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Award.Core.Entities
{
    public class Teams : BaseEntity
    {
        //public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime? DeletedAt { get; set; }

        public virtual ICollection<EmployeeTeams> EmployeeTeams { get; set; }

        public virtual ICollection<CategoryTeamEntry> CategoryTeams { get; set; }

    }
}
