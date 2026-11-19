using RoomHub.Api.Common;
using RoomHub.Api.Domain;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public record CreateBookingRequest(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);

public record CreateBookingResponse(Guid Id, Guid RoomId, string Status);

public class CreateBookingHandler(RoomRepository rooms, BookingRepository bookings)
{
    public Booking Handle(Guid roomId, CreateBookingRequest request)
    {
        // 404 is returned before any validation
        var room = rooms.Get(roomId);
        if (room is null)
            throw new BusinessException("RoomNotFound", "The room does not exist.", 404);

        // R2
        if (string.IsNullOrWhiteSpace(request.BookedBy))
            throw new BusinessException("InvalidBookedBy", "BookedBy is required.", 400);

        // R3
        if (request.EndUtc <= request.StartUtc)
            throw new BusinessException("InvalidRange", "The end must be after the start.", 400);

        // R4
        if (request.Attendees <= 0)
            throw new BusinessException("InvalidAttendees", "Attendees must be positive.", 400);

        if (request.Attendees > room.Capacity)
            throw new BusinessException("AttendeesExceedCapacity", "The room does not have enough seats.", 400);

        // R7
        foreach (var existing in bookings.ListForRoom(roomId))
        {
            if (existing.StartUtc <= request.EndUtc && request.StartUtc <= existing.EndUtc)
                throw new BusinessException("RoomOccupied", "The room is occupied in this interval.", 409);
        }

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = roomId,
            BookedBy = request.BookedBy.Trim(),
            StartUtc = request.StartUtc,
            EndUtc = request.EndUtc,
            Attendees = request.Attendees,
            Status = BookingStatus.Active
        };

        bookings.Add(booking);
        return booking;
    }
}

public static class CreateBookingEndpoint
{
    public static void MapCreateBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms/{roomId:guid}/bookings",
            (Guid roomId, CreateBookingRequest request, CreateBookingHandler handler) =>
            {
                var booking = handler.Handle(roomId, request);
                return Results.Created(
                    $"/api/bookings/{booking.Id}",
                    new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status.ToString()));
            }).WithTags("Bookings");
    }
}
