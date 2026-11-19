using RoomHub.Api.Common;
using RoomHub.Api.Common.Abstractions;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void MapCancelBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{id:guid}/cancel", async (
            Guid id,
            ICommandHandler<CancelBookingCommand> handler,
            CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(new CancelBookingCommand(id), ct);
            return result.IsSuccess ? Results.NoContent() : result.Error.ToProblem();
        }).WithTags("Bookings");
    }
}
