using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Noted.Models.Authentication
{
    public class AppUser:IdentityUser<string>
    {
        public AppUser(string username,string email)
        {
            UserName = username;
            Email = email;
        }
        public AppUser() {}
        public ICollection<Note> Notes { get; set; }
        [Required]
        public DateTime Created { get; set; }

    }
}
