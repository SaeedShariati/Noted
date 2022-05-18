using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Noted.Models.Authentication
{
    public class User:IdentityUser<string>
    {
        public ICollection<Note> Notes { get; set; }
        [Required]
        public DateTime Created { get; set; }

    }
}
