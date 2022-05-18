using Noted.Models.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Noted.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }
        [Required]
        public User Ownder { get; set; }
        public string Topic { get; set; }
        public string Text { get; set; }
        //public string[] Tags { get; set; }
        public DateTime Created { get; set; }

    }
}
