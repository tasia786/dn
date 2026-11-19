using RoomHub.Api.Common.Abstractions;
using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, CreateBookingResponse>
{
    public async Task<Result<CreateBookingResponse>> HandleAsync(CreateBookingCommand command, CancellationToken ct = default)
    {
        // 404 before any validation
        var room = await rooms.GetByIdAsync(command.RoomId, ct);
        if (room is null)
            return RoomErrors.NotFound;

        var existing = await bookings.GetActiveForRoomAsync(room.Id, ct);
        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

        // All business rules (R2–R7) live in the domain
        var result = Booking.Create(
            room, command.BookedBy, command.StartUtc, command.EndUtc, command.Attendees, nowUtc, existing);
        if (result.IsFailure)
            return result.Error;

        var booking = result.Value;
        bookings.Add(booking);
        await unitOfWork.SaveChangesAsync(ct);

        return new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status);
    }
}
