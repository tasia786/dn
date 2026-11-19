using RoomHub.Api.Common;
using RoomHub.Api.Domain;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public record CreateRoomRequest(string? Name, int Capacity);

public record CreateRoomResponse(Guid Id, string Name, int Capacity);

public class CreateRoomHandler(RoomRepository rooms)
{
    public Room Handle(CreateRoomRequest request)
    {
        // R1
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new BusinessException("InvalidName", "Room name is required.", 400);

        if (request.Capacity <= 0)
            throw new BusinessException("InvalidCapacity", "Capacity must be positive.", 400);

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Capacity = request.Capacity
        };

        rooms.Add(room);
        return room;
    }
}

public static class CreateRoomEndpoint
{
    public static void MapCreateRoom(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms", (CreateRoomRequest request, CreateRoomHandler handler) =>
        {
            var room = handler.Handle(request);
            return Results.Created(
                $"/api/rooms/{room.Id}",
                new CreateRoomResponse(room.Id, room.Name, room.Capacity));
        }).WithTags("Rooms");
    }
}
