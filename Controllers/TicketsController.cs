using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackIt.API.Models;
using TrackIt.API.Models.TransferObjects;
using TrackIt.API.Repository;

namespace TrackIt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly TicketsRepository _repository;

        public TicketsController(DataContext context)
        {
            _repository = new TicketsRepository(context);
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets()
        {
            var tickets = await _repository.GetTickets();
            return await tickets.Select(x => TicketToDTO(x)).ToListAsync();
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDTO>> GetTicket(long id)
        {
            var ticket = await _repository.GetTicketById(id);
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

            var ticket = new Ticket
            {
                Name = ticketDto.Name,
                Description = ticketDto.Description,
                Status = ticketDto.Status,
            };

            try
            {
                var success = await _repository.UpdateTicketById(id, ticket);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (DbUpdateConcurrencyException) when (!TicketExists(id))
            {
                return NotFound();
            }
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

            var newTicket = await _repository.CreateTicket(ticket);

            // TODO: Needs tested
            return CreatedAtAction(nameof(GetTicket), new { id = newTicket.Id }, TicketToDTO(ticket));
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(long id)
        {
            if (await _repository.DeleteTicketById(id))
            {
                return NoContent();
            }

            return NotFound();
        }

        private bool TicketExists(long id)
        {
            return _repository.GetTicketById(id) != null;
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
