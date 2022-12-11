using Microsoft.EntityFrameworkCore;
using TrackIt.API.Contracts;
using TrackIt.API.Models;
using TrackIt.API.Models.TransferObjects;
using TrackIt.API.Repository;

namespace TrackIt.API.Services;

public class TicketsService
{
    private TicketsRepository _repository { get; set; }

    public TicketsService(DataContext context)
    {
        _repository = new TicketsRepository(context);
    }

    public async Task<TicketDTO?> GetTicketById(long id)
    {
        var ticket = await _repository.GetTicketById(id);
        if (ticket == null)
        {
            // this is gross, would rather this be an exception
            return null;
        }
        return ticket.ToDto();
    }

    public async Task<IEnumerable<TicketDTO>> GetTickets()
    {
        var tickets = await _repository.GetTickets();
        return await tickets.Select(x => x.ToDto()).ToListAsync();
    }

    public async Task<bool> UpdateTicketById(long id, TicketDTO ticketDto)
    {
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
                return false;
            }

            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public async Task<Ticket> CreateTicket(TicketDTO ticketDto)
    {
        var ticket = new Ticket
        {
            Name = ticketDto.Name,
            Description = ticketDto.Description,
            Status = ticketDto.Status
        };
        return await _repository.CreateTicket(ticket);
    }

    public async Task<bool> DeleteTicketById(long id)
    {
        return await _repository.DeleteTicketById(id);
    }
}