using Docker.Membership.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Docker.Membership.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, Guid,
        UserClaim, UserRole, UserLogin, RoleClaim, UserToken> , IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<Role>().Property(p => p.Id).ValueGeneratedOnAdd();
        }

        public DbSet<Role> AspNetRoles { get; set; }
        public DbSet<UserRole> AspNetUserRoles { get; set; }
        public DbSet<ApplicationUser> AspNetUsers { get; set; }
    }
}
