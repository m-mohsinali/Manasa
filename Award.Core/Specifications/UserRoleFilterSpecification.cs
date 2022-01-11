using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Specifications
{
    public class UserRoleFilterSpecification : BaseSpecification<UserRole>
    {
        public UserRoleFilterSpecification(long userId) : base(a => a.UserId == userId)
        {
            AddInclude(a => a.User);
            AddInclude(a => a.Role);
        }
    }
}
