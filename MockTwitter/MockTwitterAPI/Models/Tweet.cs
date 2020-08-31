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
        public long UserId { get; set; }
        [Required]
        public string TextContent { get; set; }
        public DateTime CreatedAt {get; set;}
    }
}
