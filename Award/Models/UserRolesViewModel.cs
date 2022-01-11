using Award.Core.Entities;
using Award.Web.Controllers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{

    public class UserRolesViewModel
    {
        
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string SectorName { get; set; }
        public List<RoleViewModel> UserRoles { get; set; }
        public List<RoleViewModel> AllRoles { get; set; }
        //public string RolesNameList { get; set; }
        //public string RolesNameList => UserRoles?.Count() > 0 ? string.Join(',', this.UserRoles.Select(r => r.Name).ToList()) : null;
        public string RolesNameList { get; set; }

    }
}
