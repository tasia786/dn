using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class BookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await db.Bookings.FindAsync([id], ct);

    public Task<List<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken ct = default) =>
        db.Bookings
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
            .ToListAsync(ct);

    public Task<List<Booking>> GetActiveForBookedByAsync(string bookedBy, CancellationToken ct = default) =>
        db.Bookings
            .Where(b => b.BookedBy == bookedBy && b.Status == BookingStatus.Active)
            .ToListAsync(ct);

    public void Add(Booking booking) => db.Bookings.Add(booking);
}
