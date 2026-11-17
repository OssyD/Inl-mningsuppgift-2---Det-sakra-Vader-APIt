using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Infrastructure.Data;

public class WeatherDbContext : DbContext
{
    public DbSet<WeatherDataPoint> WeatherDataPoints => Set<WeatherDataPoint>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseSqlServer(
            configuration.GetConnectionString("WeatherDb"),
            sql => sql.EnableRetryOnFailure());
    }

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
