using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MockTwitterAPI.Models;

namespace MockTwitterAPI.Models
{
    public class Tweet
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [ForeignKey("User")]
        public String UserId { get; set; }
        [Required]
        public String Text_content { get; set; }
        public DateTime Created_at {get; set;}
    }
}
