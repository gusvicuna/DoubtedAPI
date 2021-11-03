using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoubtedAPI.Models;

namespace DoubtedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly FriendshipContext _friendshipContext;

        public UsersController(UserContext context, FriendshipContext friendshipContext)
        {
            _context = context;
            _friendshipContext = friendshipContext;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/Gus
        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var users = await _context.Users.Where(i => i.Username == username).ToListAsync();

            if (users.Count == 0)
            {
                return NotFound();
            }

            else return users[0];
        }

        // GET: api/Users/1/friends
        [HttpGet("{id}/friends")]
        public async Task<ActionResult<IEnumerable<Friendship>>> GetFriends(long id) {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return BadRequest();

            var friends = await _friendshipContext.Friendships.Where(f => (f.InvitatorId == id | f.InvitatedId == id ) & f.Accepted).Include(f => f.InvitatorUser).Include(f => f.InvitatedUser).ToListAsync();

            return friends;
        }

        // GET: api/Users/1/friendNotifications
        [HttpGet("{id}/friendNotifications")]
        public async Task<ActionResult<IEnumerable<Friendship>>> GetFriendNotifications(long id) {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var friends = await _friendshipContext.Friendships.Where(f => f.InvitatedId == id & !f.Accepted).Include(f => f.InvitatorUser).ToListAsync();

            return friends;
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

        // PUT: api/Users/5/friends/2
        [HttpPut("{id}/friends/{id2}")]
        public async Task<IActionResult> AcceptFriendship(long id, long id2) {

            var user = await _context.Users.FindAsync(id);
            if (user == null) return BadRequest();

            var user2 = await _context.Users.FindAsync(id2);
            if (user2 == null) return BadRequest();

            var friends = await _friendshipContext.Friendships.Where(f => ((f.InvitatorId == id & f.InvitatedId == id2) | (f.InvitatedId == id & f.InvitatorId == id2)) & !f.Accepted).ToListAsync();
            if (friends.Count == 0) return BadRequest();

            var friendship = friends[0];
            friendship.Accepted = true;

            _friendshipContext.Entry(friendship).State = EntityState.Modified;

            try {
                await _friendshipContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!UserExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

        // POST: api/Users/1/friends
        [HttpPost("{id}/friends")] 
        public async Task<ActionResult<User>> PostUser(long id, User friendUser)
        {

            var user = await _context.Users.FindAsync(id); 
            if (user == null) return NotFound();

            Friendship friendship = new Friendship
            {
                InvitatorId = id,
                InvitatorUser = user,
                InvitatedId = friendUser.Id,
                InvitatedUser = friendUser
            };
            _friendshipContext.Friendships.Add(friendship);
            await _friendshipContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFriendNotifications), new { id = friendship.Id }, friendship);
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
    }
}
