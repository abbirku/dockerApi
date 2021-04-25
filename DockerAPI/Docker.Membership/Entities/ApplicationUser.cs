using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Docker.Membership.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string ImageUrl { get; set; }
        public string FullName { get; set; }

        public ApplicationUser()
                    : base()
        {

        }

        internal ApplicationUser(string userName)
            : base(userName)
        {

        }
    }
}
