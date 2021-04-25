using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.Membership.DTO
{
    public class AuthenticationResponseDTO
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool IsSuccess { get; set; }
        public IList<string> Role { get; set; }
    }
}
