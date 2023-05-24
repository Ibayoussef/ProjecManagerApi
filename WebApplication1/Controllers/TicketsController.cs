using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.DB;
using WebApplication1.Dtos;
using Microsoft.AspNetCore.Cors;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class TicketsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TicketsController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets(string? query, string? status, string? assignee)
        {
            IQueryable<Ticket> ticketsQuery = _context.Tickets;

            if (!string.IsNullOrEmpty(query))
            {
                ticketsQuery = ticketsQuery.Where(t => t.title.ToLower().Contains(query));
            }

            if (!string.IsNullOrEmpty(status))
            {
                ticketsQuery = ticketsQuery.Where(t => t.Status == status);
            }

            if (!string.IsNullOrEmpty(assignee))
            {
                ticketsQuery = ticketsQuery.Where(t => t.Assignee == assignee);
            }

            var tickets = await ticketsQuery.ToListAsync();

            if (string.IsNullOrEmpty(query) && string.IsNullOrEmpty(status) && string.IsNullOrEmpty(assignee))
            {
                tickets = await _context.Tickets.ToListAsync();
            }

            return tickets;
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
            if (ticketDto.Summary != null)
            {
                ticket.Summary = ticketDto.Summary;
            }
            if (ticketDto.Assignee != null)
            {
                ticket.Assignee = ticketDto.Assignee;
            }
            if (ticketDto.Priority != null)
            {
                ticket.Priority = ticketDto.Priority;
            }
            if (ticketDto.Type != null)
            {
                ticket.Type = ticketDto.Type;
            }
            if (ticketDto.Status != null)
            {
                ticket.Status = ticketDto.Status;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok("edited");
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
            string summary = ticketDto.Description.Length > 100 ? ticketDto.Description.Substring(0, 100) : ticketDto.Description;

            var ticket = new Ticket
            {
                title = ticketDto.title,
                Description = ticketDto.Description,
                Summary = summary,
                Type = ticketDto.Type,
                Priority = ticketDto.Priority,
                Assignee = ticketDto.Assignee,
                Color = "#E64A19",
                Tags = ticketDto.Tags,
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
