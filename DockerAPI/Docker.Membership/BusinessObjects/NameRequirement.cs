using Microsoft.AspNetCore.Authorization;

namespace Docker.Membership.BusinessObjects
{
    public class NameRequirement : IAuthorizationRequirement
    {
        public NameRequirement()
        {
        }
    }
}
