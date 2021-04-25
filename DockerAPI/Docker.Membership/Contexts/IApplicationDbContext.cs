using Docker.Membership.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.Membership.Contexts
{
    public interface IApplicationDbContext
    {
        DbSet<Role> AspNetRoles { get; set; }
        DbSet<UserRole> AspNetUserRoles { get; set; }
        DbSet<ApplicationUser> AspNetUsers { get; set; }
    }
}
