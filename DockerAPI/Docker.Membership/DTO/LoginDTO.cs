using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Docker.Membership.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
