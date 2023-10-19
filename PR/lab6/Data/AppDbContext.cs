using Microsoft.EntityFrameworkCore;
using lab6.Models;

namespace lab6.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ElectroScooter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.BatteryLevel).IsRequired();
        });
    }

    public DbSet<ElectroScooter> Scooters { get; set; }
}

