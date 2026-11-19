using RoomHub.Api.Common.Abstractions;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed class GetBookingHandler(IBookingRepository bookings)
    : IQueryHandler<GetBookingQuery, GetBookingResponse>
{
    public async Task<Result<GetBookingResponse>> HandleAsync(GetBookingQuery query, CancellationToken ct = default)
    {
        var b = await bookings.GetByIdAsync(query.Id, ct);
        if (b is null)
            return BookingErrors.NotFound;

        return new GetBookingResponse(b.Id, b.RoomId, b.BookedBy, b.StartUtc, b.EndUtc, b.Attendees, b.Status);
    }
}
