using RoomHub.Api.Common.Abstractions;
using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CancelBookingCommand>
{
    public async Task<Result> HandleAsync(CancelBookingCommand command, CancellationToken ct = default)
    {
        var booking = await bookings.GetByIdAsync(command.BookingId, ct);
        if (booking is null)
            return BookingErrors.NotFound;

        // R8 + R11 live in the domain; "now" comes from TimeProvider
        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        var result = booking.Cancel(nowUtc);
        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
