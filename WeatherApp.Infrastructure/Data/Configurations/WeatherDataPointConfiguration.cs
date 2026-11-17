using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Infrastructure.Data.Configurations;

public class WeatherDataPointConfiguration : IEntityTypeConfiguration<WeatherDataPoint>
{
    public void Configure(EntityTypeBuilder<WeatherDataPoint> builder)
    {
        builder.ToTable("WeatherDataPoints");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.StationName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Timestamp)
            .HasColumnType("datetime2(0)");

        builder.Property(p => p.Temperature)
            .HasPrecision(6, 3);

        builder.Property(p => p.Humidity)
            .HasPrecision(5, 2);

        builder.Property(p => p.DataSource)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(p => new { p.StationName, p.Timestamp })
            .IsUnique();
    }
}
