using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace Docker.Membership.DTO
{
    public class RegistrationDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
