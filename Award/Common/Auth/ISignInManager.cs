using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Common.Auth
{
    public interface ISignInManager
    {
        Task SignInAsync(LdapUser user, string roleNames, User Systemuser);

        Task SignOutAsync();
    }
}
