using Award.Core.Entities;
using Award.Web.Controllers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class RoleViewModel
    {
        //IStringLocalizer<CategoriesController> _localizer;
        ////public UserRolesViewModel(IStringLocalizer<CategoriesController> localizer )
        ////{
        ////    _localizer = localizer;
        ////}
        public RoleViewModel()
        {
        }
        public RoleViewModel(Role role)
        {
            if (role != null)
            {
                this.Id = role.Id;
                this.Name = role.Name;
            }
        }
        public RoleViewModel(Role role, bool isSelected)
        {
            if (role != null)
            {
                this.Id = role.Id;
                this.Name = role.Name;
                this.IsSeleclted = isSelected;
            }
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsSeleclted { get; set; }
    }
}
