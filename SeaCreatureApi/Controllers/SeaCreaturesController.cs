using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeaCreatureApi.Data;
using SeaCreatureApi.Models;

namespace SeaCreatureApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeaCreaturesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeaCreaturesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SeaCreatures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeaCreature>>> GetSeaCreatures()
        {
            return await _context.SeaCreatures.ToListAsync();
        }

        // GET: api/SeaCreatures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeaCreature>> GetSeaCreature(int id)
        {
            var seaCreature = await _context.SeaCreatures.FindAsync(id);

            if (seaCreature == null)
            {
                return NotFound();
            }

            return seaCreature;
        }

        // PUT: api/SeaCreatures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeaCreature(int id, SeaCreature seaCreature)
        {
            if (id != seaCreature.Id)
            {
                return BadRequest();
            }

            _context.Entry(seaCreature).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeaCreatureExists(id))
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

        // POST: api/SeaCreatures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SeaCreature>> PostSeaCreature(SeaCreature seaCreature)
        {
            _context.SeaCreatures.Add(seaCreature);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeaCreature", new { id = seaCreature.Id }, seaCreature);
        }

        // DELETE: api/SeaCreatures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeaCreature(int id)
        {
            var seaCreature = await _context.SeaCreatures.FindAsync(id);
            if (seaCreature == null)
            {
                return NotFound();
            }

            _context.SeaCreatures.Remove(seaCreature);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeaCreatureExists(int id)
        {
            return _context.SeaCreatures.Any(e => e.Id == id);
        }
    }
}
