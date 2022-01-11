using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class UserRoles
    {
        public long RoleId { get; set; }
        public string  RoleName { get; set; }
        public string EmpImage { get; set; }
        public string EmployeeName { get; set; }
        public string SelectedRole { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }


    }
}
