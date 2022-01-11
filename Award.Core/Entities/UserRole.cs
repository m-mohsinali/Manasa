using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Entities
{
    public class UserRole : BaseEntity
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
