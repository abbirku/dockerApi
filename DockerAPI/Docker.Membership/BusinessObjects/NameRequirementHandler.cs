using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Docker.Membership.BusinessObjects
{
    public class NameRequirementHandler :
          AuthorizationHandler<NameRequirement>
    {
        protected override Task HandleRequirementAsync(
               AuthorizationHandlerContext context,
               NameRequirement requirement)
        {
            var claim = context.User.FindFirst("FullName");
            if (claim != null && !string.IsNullOrWhiteSpace(claim.Value))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
