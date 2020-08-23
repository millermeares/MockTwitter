using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MockTwitterAPI.Models;

namespace MockTwitterAPI.Models
{
    public class Tweet
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("User")]
        public String UserId { get; set; }
        public String Text { get; set; }
    }
}
