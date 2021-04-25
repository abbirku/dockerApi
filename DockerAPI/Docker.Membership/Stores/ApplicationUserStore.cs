using Docker.Membership.Contexts;
using Docker.Membership.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.Membership.Stores
{ //IdentityUserRole
    public class ApplicationUserStore<TUser, TRole, TContext, TKey> : UserStore<TUser, TRole, DbContext, Guid> 
        where TUser : IdentityUser<Guid>
        where TRole : IdentityRole<Guid>
        where TContext : ApplicationDbContext
        where TKey : IEquatable<TKey>
    {
        public ApplicationUserStore(DbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
            
        }
    }
}
