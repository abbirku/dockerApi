using Docker.Membership.DTO;
using Docker.Membership.Entities;
using Docker.Membership.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Docker.Membership.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<ApplicationUser> userManger,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userManger = userManger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthenticationResponseDTO> LoginUserAsync(LoginDTO loginDTO)
        {
            if (string.IsNullOrWhiteSpace(loginDTO.Username) && string.IsNullOrWhiteSpace(loginDTO.Email))
                throw new Exception("Please provide username or email.");

            ApplicationUser user = null;

            if (!string.IsNullOrWhiteSpace(loginDTO.Username))
            {
                user = await _userManger.FindByNameAsync(loginDTO.Username);
                if (user == null)
                {
                    user = await _userManger.FindByEmailAsync(loginDTO.Email);
                    if (user == null)
                        throw new Exception($"There is no user with {loginDTO.Email} email and {loginDTO.Username}");
                }
            }

            var result = await _userManger.CheckPasswordAsync(user, loginDTO.Password);
            if (!result)
                throw new Exception("Invalid password");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var tokenHandler = new JwtSecurityTokenHandler();
            var myClaims = (await _userManger.GetClaimsAsync(user)).ToList().ToArray();
            var roles = await _userManger.GetRolesAsync(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(myClaims),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenAsString = tokenHandler.WriteToken(token);

            return new AuthenticationResponseDTO
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                IsSuccess = true,
                Token = tokenAsString,
                ExpireDate = token.ValidTo,
                Role = roles
            };
        }

        public async Task<bool> RegisterUserAsync(RegistrationDTO registrationDTO)
        {
            if (registrationDTO == null)
                throw new Exception("Reigster Model is null");

            if (registrationDTO.Password != registrationDTO.ConfirmPassword)
                throw new Exception("Confirm password doesn't match the password");

            var user = await _userManger.FindByEmailAsync(registrationDTO.Email);
            if (user != null)
                throw new Exception($"A user already exists with {registrationDTO.Email} email");

            user = await _userManger.FindByNameAsync(registrationDTO.Username);
            if (user != null)
                throw new Exception($"A user already exists with {registrationDTO.Username} username");

            var applicationUser = new ApplicationUser
            {
                Email = registrationDTO.Email,
                UserName = registrationDTO.Username,
                FullName = registrationDTO.FullName,
                ImageUrl = registrationDTO.ImageUrl
            };

            var result = await _userManger.CreateAsync(applicationUser, registrationDTO.Password);

            user = await _userManger.FindByEmailAsync(registrationDTO.Email);
            await _userManger.AddToRoleAsync(user, Config.User);
            await _userManger.AddClaimsAsync(user, new Claim[] { new Claim(ClaimTypes.Role, Config.User) });
            await _userManger.AddClaimsAsync(user, new Claim[] { new Claim(Config.UserId, user.Id.ToString()) });

            return result.Succeeded;
        }

        public Guid GatherClaimInformation(string claimType)
        {
            try
            {
                if (_httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var data = claims.FirstOrDefault(x => x.Type == claimType);

                    if (data == null)
                        throw new Exception($"No claim found with type {claimType}");

                    return new Guid(data.Value);
                }
                else
                    throw new Exception("Can not retrive user identity from token");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
