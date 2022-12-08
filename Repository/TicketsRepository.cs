using Microsoft.EntityFrameworkCore;
using TrackIt.API.Contracts;
using TrackIt.API.Models;
using TrackIt.API.Models.TransferObjects;

namespace TrackIt.API.Repository;

public class TicketsRepository : ITicketsRepository
{
    private readonly DataContext _context;

    public TicketsRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<Ticket>> GetTickets()
    {
        var tickets = await _context.Tickets.ToListAsync();
        return tickets.AsQueryable();
    }

    public async Task<Ticket?> GetTicketById(long id)
    {
        return await _context.Tickets.FindAsync(id);
    }

    public async Task<bool> UpdateTicketById(long id, Ticket ticket)
    {
        var ticketItem = await _context.Tickets.FindAsync(id);
        if (ticketItem == null)
        {
            return false;
        }

        ticketItem.Name = ticket.Name;
        ticketItem.Description = ticket.Description;
        ticketItem.Status = ticket.Status;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteTicketById(long id)
    {
        var t = await _context.Tickets.FindAsync(id);
        if (t == null)
        {
            return false;
        }

        _context.Tickets.Remove(t);
        if (await _context.SaveChangesAsync() > 0)
        {
            return true;
        }

        return false;
    }

    public async Task<Ticket> CreateTicket(Ticket ticket)
    {
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }
}