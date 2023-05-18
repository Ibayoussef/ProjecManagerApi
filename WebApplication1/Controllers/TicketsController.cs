using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.DB;
using WebApplication1.Dtos;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TicketsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/Tickets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, TicketUpdateDto ticketDto)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            // Only update fields if a new value was provided in the DTO
            if (ticketDto.title != null)
            {
                ticket.title = ticketDto.title;
            }
            if (ticketDto.Description != null)
            {
                ticket.Description = ticketDto.Description;
            }
            if (ticketDto.Status != null)
            {
                ticket.Status = ticketDto.Status;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // POST: api/Tickets
        [HttpPost]
        public async Task<ActionResult<TicketReadDto>> PostTicket(TicketCreateDto ticketDto)
        {
            var project = await _context.Projects.FindAsync(ticketDto.ProjectId);

            if (project == null)
            {
                return BadRequest("Invalid Project Id");
            }

            var ticket = new Ticket
            {
                title = ticketDto.title,
                Description = ticketDto.Description,
                Status = "ToDo",
                ProjectId = ticketDto.ProjectId,
                Project = project
            };
            var ticketReadDto = new TicketReadDto
            {
                id = ticket.id,
                title = ticket.title,
                Description = ticket.Description,
                Status = "ToDo",
                ProjectId = ticket.ProjectId
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticketReadDto.id }, ticketReadDto);
        }
        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Ticket Deleted" });
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.id == id);
        }
    }
}
