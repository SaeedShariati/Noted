using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Noted.Models.ViewModels
{
    public class UserSignIn
    {
        [Required]        
        [StringLength(maximumLength:30,MinimumLength = 1)]
        public string UserName { get; set; }
        [Required]
        [StringLength(maximumLength:300,MinimumLength =5)]
        public string Password { get; set; }
        [Required]
        [StringLength(maximumLength: 300, MinimumLength = 5)]
        public string RepeatPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
