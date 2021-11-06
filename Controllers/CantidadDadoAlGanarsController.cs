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
    public class CantidadDadoAlGanarsController : ControllerBase
    {
        private readonly CantidadDadoAlGanarContext _context;

        public CantidadDadoAlGanarsController(CantidadDadoAlGanarContext context)
        {
            _context = context;
        }

        // GET: api/CantidadDadoAlGanars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CantidadDadoAlGanar>>> GetcantidadDadoAlGanar()
        {
            return await _context.cantidadDadoAlGanar.ToListAsync();
        }

        // GET: api/CantidadDadoAlGanars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CantidadDadoAlGanar>> GetCantidadDadoAlGanar(long id)
        {
            var cantidadDadoAlGanar = await _context.cantidadDadoAlGanar.FindAsync(id);

            if (cantidadDadoAlGanar == null)
            {
                return NotFound();
            }

            return cantidadDadoAlGanar;
        }

        // PUT: api/CantidadDadoAlGanars/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCantidadDadoAlGanar(long id, CantidadDadoAlGanar cantidadDadoAlGanar)
        {
            if (id != cantidadDadoAlGanar.Id)
            {
                return BadRequest();
            }

            _context.Entry(cantidadDadoAlGanar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CantidadDadoAlGanarExists(id))
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

        // POST: api/CantidadDadoAlGanars
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CantidadDadoAlGanar>> PostCantidadDadoAlGanar(CantidadDadoAlGanar cantidadDadoAlGanar)
        {
            _context.cantidadDadoAlGanar.Add(cantidadDadoAlGanar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCantidadDadoAlGanar", new { id = cantidadDadoAlGanar.Id }, cantidadDadoAlGanar);
        }

        // DELETE: api/CantidadDadoAlGanars/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CantidadDadoAlGanar>> DeleteCantidadDadoAlGanar(long id)
        {
            var cantidadDadoAlGanar = await _context.cantidadDadoAlGanar.FindAsync(id);
            if (cantidadDadoAlGanar == null)
            {
                return NotFound();
            }

            _context.cantidadDadoAlGanar.Remove(cantidadDadoAlGanar);
            await _context.SaveChangesAsync();

            return cantidadDadoAlGanar;
        }

        private bool CantidadDadoAlGanarExists(long id)
        {
            return _context.cantidadDadoAlGanar.Any(e => e.Id == id);
        }
    }
}
