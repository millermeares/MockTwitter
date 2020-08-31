using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockTwitterAPI.Models
{
    public class SecureToken
    {
        public long UserId { get; set; }
        public string IdToken { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime LastAuthed { get; set; }
        public SecureToken()
        {

        }
        public SecureToken(long UserId)
        {
            this.UserId = UserId;
            IdToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            ExpiresIn = 200;
            LastAuthed = DateTime.Now;
        }
    }
}
