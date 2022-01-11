using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Award.Core.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        //public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public long IdEmployee { get; set; }
        
        [ForeignKey("IdEmployee")]
        public virtual Employee Employee { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
