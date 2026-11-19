using RoomHub.Api.Common;
using RoomHub.Api.Domain;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public class CancelBookingHandler(BookingRepository bookings)
{
    public void Handle(Guid id)
    {
        var booking = bookings.Get(id);
        if (booking is null)
            throw new BusinessException("BookingNotFound", "The booking does not exist.", 404);

        booking.Status = BookingStatus.Cancelled;
    }
}

public static class CancelBookingEndpoint
{
    public static void MapCancelBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{id:guid}/cancel", (Guid id, CancelBookingHandler handler) =>
        {
            handler.Handle(id);
            return Results.NoContent();
        }).WithTags("Bookings");
    }
}
