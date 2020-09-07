using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MockTwitterAPI.Models
{
    public class AuthModel
    {
        public AuthModel()
        {
            
        }
        public static int validToken(User user, string idToken)
        {
            if(idToken == null || user == null)
            {
                return StatusCodes.Status400BadRequest;
            }
            if(idToken != user.IdToken)
            {
                return StatusCodes.Status401Unauthorized;
            }
            TimeSpan exp = new TimeSpan(0, user.TokenExpiresIn, 0);
            // expired.
            if (user.LastAuthed.Add(exp) < DateTime.Now)
            {
                return StatusCodes.Status401Unauthorized;
            }

            return StatusCodes.Status200OK;
        }
    }
}
