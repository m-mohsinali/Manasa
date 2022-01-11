using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Entities
{
   public class LastLogInUser:BaseEntity
    {
        public long RoleId { get; set; }
        public string  RoleName { get; set; }
        public long UserId { get; set; }


    }
}
