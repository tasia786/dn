using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public enum BookingStatus
{
    Active,
    Cancelled
}

public sealed class Booking
{
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);
    public const int MaxActiveBookingsPerParticipantPerDay = 2;

    // Used by EF Core
    private Booking() { }

    private Booking(Guid id, Guid roomId, string bookedBy, DateTime startUtc, DateTime endUtc, int attendees)
    {
        Id = id;
        RoomId = roomId;
        BookedBy = bookedBy;
        StartUtc = startUtc;
        EndUtc = endUtc;
        Attendees = attendees;
        Status = BookingStatus.Active;
    }

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc,
        IEnumerable<Booking> existingRoomBookings,
        IEnumerable<Booking> existingParticipantBookings)
    {
        startUtc = ToUtc(startUtc);
        endUtc = ToUtc(endUtc);

        // R2
        if (string.IsNullOrWhiteSpace(bookedBy))
            return BookingErrors.InvalidBookedBy;

        // R3
        if (endUtc <= startUtc)
            return BookingErrors.InvalidRange;

        // R4
        if (attendees <= 0)
            return BookingErrors.InvalidAttendees;

        if (!room.CanHost(attendees))
            return BookingErrors.AttendeesExceedCapacity;

        // R5
        if (startUtc < nowUtc)
            return BookingErrors.StartInPast;

        // R6 (exactly 4 hours is allowed)
        if (endUtc - startUtc > MaxDuration)
            return BookingErrors.TooLong;

        // R7
        if (existingRoomBookings.Any(b => b.RoomId == room.Id && b.OverlapsWith(startUtc, endUtc)))
            return BookingErrors.RoomOccupied;

        // New requirement: max 2 active bookings per participant per UTC day (of startUtc), across all rooms
        var name = bookedBy.Trim();
        var activeSameDay = existingParticipantBookings.Count(b =>
            b.Status == BookingStatus.Active &&
            b.BookedBy == name &&
            b.StartUtc.Date == startUtc.Date);
        if (activeSameDay >= MaxActiveBookingsPerParticipantPerDay)
            return BookingErrors.DailyLimitReached;

        return new Booking(Guid.NewGuid(), room.Id, name, startUtc, endUtc, attendees);
    }

    // R7: only active bookings block; touching intervals do not overlap
    public bool OverlapsWith(DateTime startUtc, DateTime endUtc) =>
        Status == BookingStatus.Active && StartUtc < endUtc && startUtc < EndUtc;

    // R8
    public Result Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return BookingErrors.AlreadyCancelled;

        Status = BookingStatus.Cancelled;
        return Result.Success();
    }

    private static DateTime ToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}
