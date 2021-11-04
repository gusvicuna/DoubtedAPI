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
    public class GamesController : ControllerBase
    {
        private readonly GameContext _context;
        private readonly PlayerContext _contextPlayer;
        private readonly UserContext _contextUser;

        public GamesController(GameContext context)
        {
            _context = context;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Games.ToListAsync();
        }

        // GET: api/Games/GameName
        [HttpGet("{GameName}")]
        public async Task<ActionResult<Game>> GetGame(string gamename)
        {
            var game = await _context.Games.Where(i => i.GameName == gamename).ToListAsync();

            if (game == null)
            {
                return NotFound();
            }

            return game[0];
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(long id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
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

        // POST: api/Games
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            System.Diagnostics.Debug.WriteLine(game.Player1Id);
            var user1 = _contextUser.Users.FindAsync((Int32)game.Player1Id);
            /*var user2 = await _contextUser.Users.FindAsync(game.Player2Id);
            var user3 = await _contextUser.Users.FindAsync(game.Player3Id);
            var user4 = await _contextUser.Users.FindAsync(game.Player4Id);
            var user5 = await _contextUser.Users.FindAsync(game.Player5Id);

            if (user1 != null)
            {
                Player player1 = new Player
                {
                   UserId = user1.Id,
                   user = user1,
                   game = game,
                   GameId = game.Id
                };
                game.Player1 = player1;
            }*/
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGames), new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(long id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return game;
        }

        private bool GameExists(long id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
