using TrackIt.API.Models;

namespace TrackIt.API.Contracts;

public interface ITicketsRepository
{
    public Task<IQueryable<Ticket>> GetTickets();

    public Task<Ticket?> GetTicketById(long id);

    public Task<Ticket> CreateTicket(Ticket ticket);

    public Task<bool> UpdateTicketById(long id, Ticket ticket);

    public Task<bool> DeleteTicketById(long id);
}