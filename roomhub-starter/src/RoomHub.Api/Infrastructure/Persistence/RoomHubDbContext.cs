using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class RoomHubDbContext(DbContextOptions<RoomHubDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // SQLite loses DateTimeKind; always read dates back as UTC.
        configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>(room =>
        {
            room.ToTable("Rooms");
            room.HasKey(r => r.Id);
            room.Property(r => r.Name).IsRequired().HasMaxLength(200);
            room.Property(r => r.Capacity).IsRequired();
        });

        modelBuilder.Entity<Booking>(booking =>
        {
            booking.ToTable("Bookings");
            booking.HasKey(b => b.Id);
            booking.Property(b => b.BookedBy).IsRequired().HasMaxLength(200);
            booking.Property(b => b.Status).HasConversion<string>().HasMaxLength(20);
            booking.HasOne<Room>().WithMany().HasForeignKey(b => b.RoomId);
            booking.HasIndex(b => b.RoomId);
        });
    }

    private sealed class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(
        v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
}
