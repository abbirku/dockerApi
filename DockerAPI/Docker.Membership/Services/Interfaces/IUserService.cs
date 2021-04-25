using Docker.Membership.DTO;
using Docker.Membership.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.Membership.Services
{
    public interface IUserService
    {
        IList<ApplicationUserDTO> GetApplicationUsers();

    }
}
