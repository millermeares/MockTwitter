using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockTwitterAPI.Models;

namespace MockTwitterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TwitterContext _context;
        private readonly ILogger _logger;

        public UsersController(TwitterContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id:long}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            _logger.LogInformation("reached GetUserUserByID");
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        [HttpGet("check/{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            _logger.LogInformation("reached getUser By Username");
            if(!UsernameExists(username)) {
                return NotFound();
            }
            var user = await _context.Users.Where(e => e.Username == username).FirstAsync();
            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("login/")]
        public async Task<ActionResult<SecureToken>> LoginUser(User user)
        {
            _logger.LogInformation("Reached Login Point");
            var pw = user.PasswordHash;
            if(!UsernameExists(user.Username)) {
                return StatusCode(303);
            }
            var logging_in_user = await _context.Users.Where(e => e.Username == user.Username).FirstAsync();
            var salt = logging_in_user.Salt;
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pw,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if(hashed != logging_in_user.PasswordHash)
            {
                return Unauthorized();
            }
            // need to create a new id token and put it in the DB. Then return it.
            SecureToken token_new = new SecureToken(logging_in_user.Id);
            try
            {
                logging_in_user.LastAuthed = token_new.LastAuthed;
                logging_in_user.IdToken = token_new.IdToken;
                logging_in_user.TokenExpiresIn = token_new.ExpiresIn;
                await _context.SaveChangesAsync();
            } catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Could not update user when logging in");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetUser", "Users", new { id = logging_in_user.Id }, token_new);
        }
        [HttpPost("autologin/")]
        public async Task<ActionResult<SecureToken>> AutoLoginUser(SecureToken storedToken)
        {
            var token = await _context.Users.FindAsync(storedToken.UserId);
            if(token == null)
            {
                return BadRequest();
            }
            // incorrect GUID.
            if (token.IdToken != storedToken.IdToken)
            {
                return Unauthorized();
            }
            TimeSpan exp = new TimeSpan(0, token.TokenExpiresIn, 0);
            // expired.
            if(token.LastAuthed.Add(exp) < DateTime.Now)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("createaccount/")]
        public async Task<ActionResult<SecureToken>> PostUser(User user)
        {
            _logger.LogInformation("Reached Post User");
            if(user.Username.Length > 50)
            {
                return BadRequest();
            }
            // prevents duplicate usernames.
            if(UsernameExists(user.Username))
            {
                return StatusCode(303);
            }
            var pw = user.PasswordHash;
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            user.Salt = salt;

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pw,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            user.PasswordHash = hashed;

            user.CreatedAt = DateTime.Now;
            SecureToken token = new SecureToken(user.Id);
            user.IdToken = token.IdToken;
            user.TokenExpiresIn = token.ExpiresIn;
            user.LastAuthed = token.LastAuthed;
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            } catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again and see if the problem persists");
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
            token.UserId = user.Id;
            return CreatedAtAction("GetUser", "Users", new { id = user.Id }, token);

        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        private bool UsernameExists(string username) 
        {
            return _context.Users.Any(e => e.Username == username);
        }
    }
}
