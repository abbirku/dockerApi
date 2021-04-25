using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.Membership.DTO
{
    public class ApplicationUserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public int TotalDuration { get; set; }
        public decimal TotalScore { get; set; }
    }
}
