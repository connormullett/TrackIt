using TrackIt.API.Models.Enums;
using TrackIt.API.Models.TransferObjects;

namespace TrackIt.API.Models;

public class Ticket
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TicketStatus Status { get; set; }

    public TicketDTO ToDto() =>
        new TicketDTO
        {
            Id = this.Id,
            Name = this.Name,
            Description = this.Description,
            Status = this.Status
        };
}