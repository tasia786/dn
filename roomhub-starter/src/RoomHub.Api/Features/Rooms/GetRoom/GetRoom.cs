using RoomHub.Api.Common;
using RoomHub.Api.Domain;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public record GetRoomResponse(Guid Id, string Name, int Capacity);

public class GetRoomHandler(RoomRepository rooms)
{
    public Room Handle(Guid id)
    {
        var room = rooms.Get(id);
        if (room is null)
            throw new BusinessException("RoomNotFound", "The room does not exist.", 404);

        return room;
    }
}

public static class GetRoomEndpoint
{
    public static void MapGetRoom(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/rooms/{id:guid}", (Guid id, GetRoomHandler handler) =>
        {
            var room = handler.Handle(id);
            return Results.Ok(new GetRoomResponse(room.Id, room.Name, room.Capacity));
        }).WithTags("Rooms");
    }
}
