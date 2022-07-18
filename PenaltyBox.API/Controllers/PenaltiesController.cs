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
                                                                    string? referees = null,
                                                                    string? seasonType = null)
        {
            if (_context.Penalties == null)
            {
                return NotFound();
            }

            DateTime startDay = DateParser.ParseString(startDate, new DateTime(2020, 01, 01));
            DateTime endDay = DateParser.ParseString(endDate, DateTime.Today.AddDays(1));

            bool homeStatus = false;
            if (!String.IsNullOrEmpty(home))
            {
                bool successfulConversion = Boolean.TryParse(home, out homeStatus);
                if (!successfulConversion)
                {
                    home = null;
                }
            }

            List<string> opponentList = StringParser.ParseString(opponentName);
            List<string> refList = StringParser.ParseString(referees);
            List<string> penaltyList = StringParser.ParseString(penaltyName);
            List<string> playerList = StringParser.ParseString(playerName);
            List<string> teamList = StringParser.ParseString(teamName);

            List<SeasonType> seasonTypeList = StringParser.ParseSeasonType(seasonType);
            if (seasonTypeList.Count == 0)
            {
                seasonType = null;
            }

            // Need to Handle Refs
            return await _context.Penalties.Where((penalty) => String.IsNullOrEmpty(penaltyName) || penaltyList.Contains(penalty.PenaltyName))
                                           .Where((penalty) => String.IsNullOrEmpty(playerName) || playerList.Contains(penalty.Player))
                                           .Where((penalty) => String.IsNullOrEmpty(teamName) || teamList.Contains(penalty.Team))
                                           .Where((penalty) => String.IsNullOrEmpty(opponentName) || opponentList.Contains(penalty.Opponent))
                                           .Where((penalty) => startDay <= penalty.GameDate && endDay >= penalty.GameDate)
                                           .Where((penalty) => String.IsNullOrEmpty(home) || (homeStatus == penalty.Home))
                                           .Where((penalty) => String.IsNullOrEmpty(referees) || penalty.Referees.ToList().Intersect(refList).Any())
                                           .Where((penalty) => String.IsNullOrEmpty(seasonType) || seasonTypeList.Contains(penalty.SeasonType))
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

            var lastPenalty = penalties[^1];

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
