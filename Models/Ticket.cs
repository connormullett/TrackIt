using TrackIt.API.Models.Enums;

namespace TrackIt.API.Models;

public class Ticket
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TicketStatus Status { get; set; }
}