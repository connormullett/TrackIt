using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackIt.API.Models;
using TrackIt.API.Models.TransferObjects;
using TrackIt.API.Services;

namespace TrackIt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly TicketsService _service;

        public TicketsController(DataContext context)
        {
            _service = new TicketsService(context);
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets()
        {
            var tickets = await _service.GetTickets();
            return tickets.ToList();
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDTO>> GetTicket(long id)
        {
            var ticket = await _service.GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
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

            if (await _service.UpdateTicketById(id, ticketDto)) return NoContent();
            else return NotFound();
        }

        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TicketDTO>> PostTicket(TicketDTO ticketDto)
        {
            var newTicket = await _service.CreateTicket(ticketDto);
            return CreatedAtAction(nameof(GetTicket), new { id = newTicket.Id }, newTicket.ToDto());
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(long id)
        {
            if (await _service.DeleteTicketById(id))
            {
                return NoContent();
            }

            return NotFound();
        }

        private bool TicketExists(long id)
        {
            return _service.GetTicketById(id) != null;
        }
    }
}
