using Docker.Membership.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Docker.Membership.Services
{
    public interface IAccountService
    {
        Task<bool> RegisterUserAsync(RegistrationDTO registrationDTO);
        Task<AuthenticationResponseDTO> LoginUserAsync(LoginDTO loginDTO);
        Guid GatherClaimInformation(string claimType);
    }
}
