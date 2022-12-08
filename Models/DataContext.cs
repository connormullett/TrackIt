using Microsoft.EntityFrameworkCore;

namespace TrackIt.API.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; } = null;
}