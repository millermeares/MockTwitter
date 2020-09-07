using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockTwitterAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Org.BouncyCastle.Bcpg;

namespace MockTwitterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly TwitterContext _context;
        private readonly ILogger<TweetsController> _logger;
        public TweetsController(TwitterContext context, ILogger<TweetsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/Tweets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object>>> GetTweets()
        {
            /*
            var query = from t in _context.Tweets
                        join u in _context.Users on t.UserId equals u.Id into grouping
                        from u in grouping.DefaultIfEmpty()
                        select new { t, u };
            
            List<TweetDisplay> list = new List<TweetDisplay>();
            foreach(var x in query)
            {
                list.Add(new TweetDisplay(x.u.Username, x.t.TextContent, x.t.CreatedAt));
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());

            return JsonConvert.SerializeObject(list, Formatting.Indented, settings);
            */
            var query = from t in _context.Tweets
                        join u in _context.Users on t.UserId equals u.Id into grouping
                        from u in grouping.DefaultIfEmpty()
                        orderby t.CreatedAt descending
                        select new { tweetId=t.Id, username=u.Username, textContent= t.TextContent, createdAt = t.CreatedAt };
            return await query.ToListAsync();
        }

        // GET: api/Tweets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tweet>> GetTweet(long id)
        {
            var tweet = await _context.Tweets.FindAsync(id);

            if (tweet == null)
            {
                return NotFound();
            }

            return tweet;
        }

        // PUT: api/Tweets/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTweet(long id, Tweet tweet)
        {
            if (id != tweet.Id)
            {
                return BadRequest();
            }

            _context.Entry(tweet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TweetExists(id))
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

        // POST: api/Tweets
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Tweet>> PostTweet([FromBody]Tweet tweet, [FromHeader] string Authorization)
        {
            string idToken = Authorization.Split(" ")[1];
            _logger.LogInformation(HttpContext.Request.ToString());
            _logger.LogInformation(idToken);   
            // validate authorization headers.
            var user = await _context.Users.FindAsync(tweet.UserId);
            var code = AuthModel.validToken(user, idToken);
            _logger.LogInformation(code.ToString());
            if(code != 200)
            {
                return StatusCode(code);
            }
            try
            {
                tweet.CreatedAt = DateTime.Now;
                _context.Tweets.Add(tweet);
                await _context.SaveChangesAsync();
            } catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Could not post tweet. Internal error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetTweet", new { id = tweet.Id }, tweet);
        }

        // DELETE: api/Tweets/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tweet>> DeleteTweet(long id)
        {
            var tweet = await _context.Tweets.FindAsync(id);
            if (tweet == null)
            {
                return NotFound();
            }

            _context.Tweets.Remove(tweet);
            await _context.SaveChangesAsync();

            return tweet;
        }

        private bool TweetExists(long id)
        {
            return _context.Tweets.Any(e => e.Id == id);
        }
    }
}
