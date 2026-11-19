using RoomHub.Api.Common;
using RoomHub.Api.Common.Abstractions;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public static void MapCreateBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms/{roomId:guid}/bookings", async (
            Guid roomId,
            CreateBookingRequest request,
            ICommandHandler<CreateBookingCommand, CreateBookingResponse> handler,
            CancellationToken ct) =>
        {
            var command = new CreateBookingCommand(
                roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees);

            var result = await handler.HandleAsync(command, ct);
            return result.IsSuccess
                ? Results.Created($"/api/bookings/{result.Value.Id}", result.Value)
                : result.Error.ToProblem();
        }).WithTags("Bookings");
    }
}
