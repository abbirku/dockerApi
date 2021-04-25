using Docker.Membership.Contexts;
using Docker.Membership.DTO;
using Docker.Membership.Entities;
using Docker.Membership.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Docker.Membership.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ApplicationUserDTO> GetApplicationUsers()
        {
            var users = (from userRole in _context.AspNetUserRoles
                         join user in _context.AspNetUsers on userRole.UserId equals user.Id
                         join role in _context.AspNetRoles on userRole.RoleId equals role.Id
                         where role.Name != Config.Admin
                         select new ApplicationUserDTO
                         {
                             Id = user.Id,
                             Email = user.Email,
                             ImageUrl = user.ImageUrl,
                             Username = user.UserName
                         }).ToList();
            
            return users;
        }
    }
}
