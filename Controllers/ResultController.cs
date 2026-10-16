
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [Route("api/results")]
    [ApiController]
    [Authorize]
    public class ResultController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/results/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Participant,Organizer,Admin")]
        public async Task<ActionResult<Result>> GetResult(int id)
        {
            var result = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST: api/results
        [HttpPost]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<ActionResult<Result>> CreateResult(Result result)
        {
            bool registrationExists = await _context.Registrations
                .AnyAsync(r => r.RegistrationID == result.RegistrationID);

            if (!registrationExists)
                return BadRequest("Registration does not exist.");

            bool resultExists = await _context.Results
                .AnyAsync(r => r.RegistrationID == result.RegistrationID);

            if (resultExists)
                return BadRequest("A result already exists for this registration.");

            if (!new[] { "completed", "dnf", "dns" }
                .Contains(result.ResultStatus.ToLower()))
            {
                return BadRequest(
                    "Result status must be completed, dnf, or dns.");
            }

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetResult),
                new { id = result.ResultID },
                result);
        }

        // PUT: api/results/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> UpdateResult(int id, Result updatedResult)
        {
            if (id != updatedResult.ResultID)
                return BadRequest("Result ID does not match.");

            var result = await _context.Results.FindAsync(id);

            if (result == null)
                return NotFound();

            bool registrationExists = await _context.Registrations
                .AnyAsync(r =>
                    r.RegistrationID == updatedResult.RegistrationID);

            if (!registrationExists)
                return BadRequest("Registration does not exist.");

            if (!new[] { "completed", "dnf", "dns" }
                .Contains(updatedResult.ResultStatus.ToLower()))
            {
                return BadRequest(
                    "Result status must be completed, dnf, or dns.");
            }

            result.RegistrationID = updatedResult.RegistrationID;
            result.FinishTime = updatedResult.FinishTime;
            result.Position = updatedResult.Position;
            result.ResultStatus = updatedResult.ResultStatus;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
