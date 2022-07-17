using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PenaltyBox.API.Data;
using PenaltyBox.API.Models;
using PenaltyBox.API.Utilities;

namespace PenaltyBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PenaltiesController : ControllerBase
    {
        private readonly PenaltyContext _context;

        public PenaltiesController(PenaltyContext context)
        {
            _context = context;
        }

        // GET: api/Penalties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Penalty>>> GetPenalties()
        {
            if (_context.Penalties == null)
            {
                return NotFound();
            }
            return await _context.Penalties.ToListAsync();
        }

        // GET: api/Penalties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Penalty>> GetPenalty(int id)
        {
            if (_context.Penalties == null)
            {
                return NotFound();
            }
            var penalty = await _context.Penalties.FindAsync(id);

            if (penalty == null)
            {
                return NotFound();
            }

            return penalty;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Penalty>>> GetFilteredPenalty(string? playerName = null,
                                                                    string? teamName = null,
                                                                    string? startDate = null,
                                                                    string? endDate = null,
                                                                    string? opponentName = null,
                                                                    string? penaltyName = null,
                                                                    string? home = null,
                                                                    string? referees = null)
        {
            if (_context.Penalties == null)
            {
                return NotFound();
            }

            DateTime startDay = DateParser.ParseString(startDate, new DateTime(2020, 01, 01));
            DateTime endDay = DateParser.ParseString(endDate, DateTime.Today.AddDays(1));

            // Need to Handle Refs
            return await _context.Penalties.Where((penalty) => String.IsNullOrEmpty(penaltyName) || penaltyName.Equals(penalty.PenaltyName))
                                           .Where((penalty) => String.IsNullOrEmpty(playerName) || playerName.Equals(penalty.Player))
                                           .Where((penalty) => String.IsNullOrEmpty(teamName) || teamName.Equals(penalty.Team))
                                           .Where((penalty) => String.IsNullOrEmpty(opponentName) || opponentName.Equals(penalty.Opponent))
                                           .Where((penalty) => startDay <= penalty.GameDate && endDay >= penalty.GameDate)
                                           .Where((penalty) => String.IsNullOrEmpty(home) || Boolean.Parse(home))
                                           .ToListAsync();
        }

        // PUT: api/Penalties/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPenalty(int id, Penalty penalty)
        {
            if (id != penalty.Id)
            {
                return BadRequest();
            }

            _context.Entry(penalty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PenaltyExists(id))
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

        // POST: api/Penalties
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddPenalty")]
        public async Task<ActionResult<Penalty>> PostPenalty(Penalty penalty)
        {
            if (_context.Penalties == null)
            {
                return Problem("Entity set 'PenaltyContext.Penalties'  is null.");
            }

            // Set the date to Midnight.
            penalty.GameDate = penalty.GameDate.Date;

            _context.Penalties.Add(penalty);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPenalty", new { id = penalty.Id }, penalty);
        }

        [HttpPost("AddPenalties")]
        public async Task<ActionResult<Penalty>> PostPenalties(List<Penalty> penalties)
        {
            if (_context.Penalties == null)
            {
                return Problem("Entity set 'PenaltyContext.Penalties'  is null.");
            }

            foreach(Penalty penalty in penalties)
            {
                penalty.GameDate = penalty.GameDate.Date;
            }

            _context.Penalties.AddRange(penalties);
            await _context.SaveChangesAsync();

            var lastPenalty = penalties[penalties.Count - 1];

            return CreatedAtAction("GetPenalty", new { id = lastPenalty.Id }, lastPenalty);
        }

        // DELETE: api/Penalties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePenalty(int id)
        {
            if (_context.Penalties == null)
            {
                return NotFound();
            }
            var penalty = await _context.Penalties.FindAsync(id);
            if (penalty == null)
            {
                return NotFound();
            }

            _context.Penalties.Remove(penalty);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PenaltyExists(int id)
        {
            return (_context.Penalties?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
