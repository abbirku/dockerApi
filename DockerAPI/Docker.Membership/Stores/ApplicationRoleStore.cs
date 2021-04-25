using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Docker.Membership.Stores
{
    public class ApplicationRoleStore<TRole> : RoleStore<TRole, DbContext, Guid> where TRole : IdentityRole<Guid>
    {
        public ApplicationRoleStore(DbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {

        }
    }
}
