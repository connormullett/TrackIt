using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackIt.API.Models;
using TrackIt.API.Models.TransferObjects;

namespace TrackIt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly DataContext _context;

        public TicketsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets()
        {
            return await _context.Tickets
                .Select(x => TicketToDTO(x))
                .ToListAsync();

        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDTO>> GetTicket(long id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return TicketToDTO(ticket);
        }

        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(long id, TicketDTO ticketDto)
        {
            if (id != ticketDto.Id)
            {
                return BadRequest();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Name = ticketDto.Name;
            ticket.Description = ticketDto.Description;
            ticket.Status = ticketDto.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TicketExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TicketDTO>> PostTicket(TicketDTO ticketDto)
        {
            var ticket = new Ticket
            {
                Name = ticketDto.Name,
                Description = ticketDto.Description,
                Status = ticketDto.Status
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, TicketToDTO(ticket));
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(long id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(long id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

        private static TicketDTO TicketToDTO(Ticket ticket) =>
            new TicketDTO
            {
                Id = ticket.Id,
                Name = ticket.Name,
                Description = ticket.Description,
                Status = ticket.Status
            };
    }
}
