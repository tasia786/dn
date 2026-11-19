using RoomHub.Api.Common.Abstractions;
using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(IBookingRepository bookings, IUnitOfWork unitOfWork)
    : ICommandHandler<CancelBookingCommand>
{
    public async Task<Result> HandleAsync(CancelBookingCommand command, CancellationToken ct = default)
    {
        var booking = await bookings.GetByIdAsync(command.BookingId, ct);
        if (booking is null)
            return BookingErrors.NotFound;

        // R8 lives in the domain
        var result = booking.Cancel();
        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
