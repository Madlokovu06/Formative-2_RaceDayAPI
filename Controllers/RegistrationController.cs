
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using System.Security.Claims;

namespace RaceDayAPI.Controllers
{
    [Route("api/registrations")]
    [ApiController]
    [Authorize]
    public class RegistrationController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public RegistrationController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/registrations
        [HttpPost]
        [Authorize(Roles = "Participant")]
        public async Task<ActionResult<Registration>> RegisterForEvent(
            Registration registration)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            bool eventExists = await _context.Events
                .AnyAsync(e => e.EventID == registration.EventID);

            if (!eventExists)
                return BadRequest("Event does not exist.");

            bool categoryExists = await _context.Categories
                .AnyAsync(c =>
                    c.CategoryID == registration.CategoryID &&
                    c.EventID == registration.EventID);

            if (!categoryExists)
                return BadRequest(
                    "Category does not exist for this event.");

            bool alreadyRegistered = await _context.Registrations
                .AnyAsync(r =>
                    r.UserID == userId &&
                    r.EventID == registration.EventID);

            if (alreadyRegistered)
                return BadRequest(
                    "You are already registered for this event.");

            registration.UserID = userId;
            registration.RegistrationDate = DateTime.Now;
            registration.Status = "Registered";

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetRegistration),
                new { id = registration.RegistrationID },
                registration);
        }

        // GET: api/registrations/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Participant,Organizer,Admin")]
        public async Task<ActionResult<Registration>> GetRegistration(int id)
        {
            var registration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.RegistrationID == id);

            if (registration == null)
                return NotFound();

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            bool isParticipant = User.IsInRole("Participant");
            bool isPrivileged = User.IsInRole("Organizer")
                || User.IsInRole("Admin");

            if (isParticipant && registration.UserID != userId)
                return Forbid();

            if (!isParticipant && !isPrivileged)
                return Forbid();

            return Ok(registration);
        }
    }
}
