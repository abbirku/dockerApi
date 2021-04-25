using System;
using Microsoft.AspNetCore.Identity;

namespace Docker.Membership.Entities
{
    public class UserToken
        : IdentityUserToken<Guid>
    {

    }
}
