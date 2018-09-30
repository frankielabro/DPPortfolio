using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.ViewModels
{
   
    public class AdminRegistrationViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(8, MinimumLength=2, ErrorMessage ="You must specify a password between 4 and 8 characters")]
        public string Password { get; set; }
    }
}
