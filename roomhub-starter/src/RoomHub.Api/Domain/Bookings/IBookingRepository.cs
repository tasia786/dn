namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken ct = default);
    Task<List<Booking>> GetActiveForBookedByAsync(string bookedBy, CancellationToken ct = default);
    void Add(Booking booking);
}
