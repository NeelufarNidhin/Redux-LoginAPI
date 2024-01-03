using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ReduxJWTLogin.Models
{
	public class ApplicationUser : IdentityUser
	{
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName{ get; set; }

       

        //public string? RefreshToken { get; set; }
        //public DateTime RefreshTokenExpiryTime { get; set; }
    }
}

