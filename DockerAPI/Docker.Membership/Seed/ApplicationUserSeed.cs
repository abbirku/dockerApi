using Docker.Membership.Contexts;
using Docker.Membership.Entities;
using Docker.Membership.Services;
using Docker.Membership.Shared;
using Docker.Membership.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Docker.Membership.Seed
{
    public interface IApplicationUserSeed
    {
        public Task SeedRollAndAdminUserAsync();
    }

    public class ApplicationUserSeed : IApplicationUserSeed
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationUserSeed(ApplicationDbContext context, 
            ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedRollAndAdminUserAsync()
        {
            try
            {
                //Seeding roles
                var roleStore = new ApplicationRoleStore<Role>(_context);
                if (!_context.Roles.Any(r => r.Name == Config.Admin))
                    await _roleManager.CreateAsync(new Role { Name = Config.Admin, NormalizedName = Config.Admin });

                if (!_context.Roles.Any(r => r.Name == Config.User))
                    await _roleManager.CreateAsync(new Role { Name = Config.User, NormalizedName = Config.User });

                //Seeding admin user
                var user = new ApplicationUser
                {
                    UserName = "adminuser",
                    NormalizedUserName = "adminuser",
                    Email = "admin@email.com",
                    NormalizedEmail = "admin@email.com",
                    PhoneNumber = "01000000001",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                if (!_context.Users.Any(u => u.UserName == user.UserName))
                {
                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(user, "dev@trackTeam1");
                    user.PasswordHash = hashed;

                    await _userManager.CreateAsync(user);
                    await _userManager.AddToRoleAsync(user, Config.Admin);
                    await _userManager.AddClaimsAsync(user, new Claim[] { new Claim(ClaimTypes.Role, Config.Admin) });
                    await _userManager.AddClaimsAsync(user, new Claim[] { new Claim(Config.UserId, user.Id.ToString()) });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
