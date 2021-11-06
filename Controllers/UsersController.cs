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
        private readonly PlayerContext _contextPlayer;
        private readonly GameContext _contextGame;
        private readonly CantidadDadoAlGanarContext _contextDado;

        public UsersController(UserContext context, FriendshipContext friendshipContext,PlayerContext playerContext, GameContext gameContext,CantidadDadoAlGanarContext cantidadDadoAlGanarContext)
        {
            _context = context;
            _friendshipContext = friendshipContext;
            _contextPlayer = playerContext;
            _contextGame = gameContext;
            _contextDado = cantidadDadoAlGanarContext;
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
            var dadosGanar = await _contextDado.cantidadDadoAlGanar.Where(u => u.myUserId == users[0].Id).ToListAsync();
            if( dadosGanar.FirstOrDefault() != null)
            {
                if(dadosGanar[0] != null)
                {
                    Console.WriteLine(dadosGanar[0]);
                    if (users[0].DadosAlGanar == null)
                    {
                        users[0].DadosAlGanar = new List<int>();
                    }
                
                    foreach(CantidadDadoAlGanar dado in dadosGanar)
                    {
                        users[0].DadosAlGanar.Add(dado.NumeroDados);
                    }
                
                }
            }
            
            

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
        // GET: api/Users/1/GameNotifications
        [HttpGet("{id}/GameNotifications")]
        public async Task<ActionResult<IEnumerable<Player>>> GetGameNotifications(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var friends = await _contextPlayer.Players.Where(u => u.UserId == id & !u.AcceptationState).ToListAsync();

            return friends;
        }
        //Get: api/Users/1/players
        [HttpGet("{id}/players")]
        public async Task<ActionResult<IEnumerable<Game>>> GetPlayers(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return BadRequest();

            var players = await _contextPlayer.Players.Where(u => u.UserId == id & u.AcceptationState).ToListAsync();
            var mygamesid = new List<long>();
            foreach(Player player in players)
            {
                mygamesid.Add(player.GameId);
            }
            var games = new List<Game>();
            foreach(long mgid in mygamesid)
            {
                var game = await _contextGame.Games.FindAsync(mgid);
                if (game == null) continue;
                games.Add(game);
            }
            return games;
        }
        //Get: api/Users/1/2/players
        [HttpGet("{id}/{gameid}/players")]
        public async Task<ActionResult<IEnumerable<Player>>> GetOnePlayer(long id, long gameid)
        {
            var player = await _contextPlayer.Players.Where(u => u.UserId == id & u.GameId == gameid).ToListAsync();
            return player;
        }

        //GET: api/Users/1/dadoslist
        [HttpGet("{id}/dadoslist")]
        public async Task<ActionResult<IEnumerable<CantidadDadoAlGanar>>> GetDadosList(long id)
        {
            var dadosGanar = await _contextDado.cantidadDadoAlGanar.Where(u => u.myUserId == id).ToListAsync();
            return dadosGanar;
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

        // DELETE: api/Users/5/friends/2
        [HttpDelete("{id}/friends/{id2}")]
        public async Task<ActionResult<Friendship>> DeleteFriendship(long id,long id2) {
            var friends = await _friendshipContext.Friendships.Where(f => 
                (f.InvitatorId == id & f.InvitatedId == id2) | (f.InvitatedId == id & f.InvitatorId == id2)).ToListAsync();
            if (friends.Count == 0) {
                return BadRequest();
            }

            _friendshipContext.Friendships.Remove(friends[0]);
            await _friendshipContext.SaveChangesAsync();

            return friends[0];
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
