using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MockTwitterAPI.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Username { get; set; } 
        [Required]
        public string PasswordHash {get; set;}
        public byte[] Salt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string IdToken { get; set; }
        public int TokenExpiresIn { get; set; }
        public DateTime LastAuthed { get; set; }
    }
}
