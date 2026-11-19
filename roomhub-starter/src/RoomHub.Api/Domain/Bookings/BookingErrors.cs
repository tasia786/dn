using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

public static class BookingErrors
{
    public static readonly Error InvalidBookedBy =
        Error.Validation("Booking.InvalidBookedBy", "BookedBy is required.");

    public static readonly Error InvalidRange =
        Error.Validation("Booking.InvalidRange", "The end must be strictly after the start.");

    public static readonly Error InvalidAttendees =
        Error.Validation("Booking.InvalidAttendees", "Attendees must be strictly positive.");

    public static readonly Error AttendeesExceedCapacity =
        Error.Validation("Booking.AttendeesExceedCapacity", "The room does not have enough seats.");

    public static readonly Error StartInPast =
        Error.Validation("Booking.StartInPast", "A booking cannot start in the past.");

    public static readonly Error TooLong =
        Error.Validation("Booking.TooLong", "A booking cannot last more than 4 hours.");

    public static readonly Error TooLongForLargeRoom =
        Error.Validation("Booking.TooLongForLargeRoom", "Rooms with a capacity greater than 10 can be booked for at most 2 hours.");

    public static readonly Error TooLateToCancel =
        Error.Conflict("Booking.TooLateToCancel", "A booking can be cancelled only if it starts at least 60 minutes from now.");

    public static readonly Error RoomOccupied =
        Error.Conflict("Booking.RoomOccupied", "The room is occupied in this interval.");

    public static readonly Error DailyLimitReached =
        Error.Conflict("Booking.DailyLimitReached", "A participant cannot have more than 2 active bookings on the same day.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict("Booking.AlreadyCancelled", "The booking is already cancelled.");

    public static readonly Error NotFound =
        Error.NotFound("Booking.NotFound", "The booking does not exist.");
}
