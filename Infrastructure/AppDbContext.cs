using Microsoft.EntityFrameworkCore;
using Pix.Domain.Entities;

public class AppDbContext : DbContext
{
    public DbSet<PixTransaction> PixTransactions
        => Set<PixTransaction>();

    public DbSet<OutboxEvent> OutboxEvents
        => Set<OutboxEvent>();

    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}