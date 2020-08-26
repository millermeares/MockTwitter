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
        public string Password_hash {get; set;}
        public byte[] Salt { get; set; }
        public DateTime Created_at { get; set; }
    }
}
