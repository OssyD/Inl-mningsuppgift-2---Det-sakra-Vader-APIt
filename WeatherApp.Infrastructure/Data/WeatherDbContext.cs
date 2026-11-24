using Microsoft.EntityFrameworkCore;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Infrastructure.Data;

public class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
{
    public DbSet<WeatherDataPoint> WeatherDataPoints => Set<WeatherDataPoint>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<WeatherDataPoint>();

        entity.Property(p => p.StationName).IsRequired().HasMaxLength(100);
        entity.Property(p => p.Timestamp).HasColumnType("datetime2(0)");
        entity.Property(p => p.Temperature).HasPrecision(6, 3);
        entity.Property(p => p.Humidity).HasPrecision(5, 2);
        entity.Property(p => p.DataSource).IsRequired().HasMaxLength(20);
        entity.HasIndex(p => new { p.StationName, p.Timestamp }).IsUnique();
    }
}
