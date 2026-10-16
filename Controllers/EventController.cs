using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RaceDayAPI.Controllers
{
    [Route("api/events")]
    [ApiController]
    [Authorize]
    public class EventController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/event
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/event/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return eventItem;
        }

        // POST: api/event
        [Authorize(Roles = "Organizer")]
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event eventItem)
        {
            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = eventItem.EventID }, eventItem);
        }

        // PUT: api/event/{id}
        [Authorize(Roles = "Organizer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event eventItem)
        {
            if (id != eventItem.EventID)
            {
                return BadRequest();
            }

            _context.Entry(eventItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return Ok(eventItem);
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventID == id);
        }

        // DELETE: api/event/{id}
        [Authorize(Roles = "Organizer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // GET: api/events/{id}/registrations
        [HttpGet("{id}/registrations")]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<ActionResult<IEnumerable<Registration>>>
            GetEventRegistrations(int id)
        {
            bool eventExists = await _context.Events
                .AnyAsync(e => e.EventID == id);

            if (!eventExists)
                return NotFound("Event does not exist.");

            var registrations = await _context.Registrations
                .Where(r => r.EventID == id)
                .ToListAsync();

            return Ok(registrations);
        }

    }
}