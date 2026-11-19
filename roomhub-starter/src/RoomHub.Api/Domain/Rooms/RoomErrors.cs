using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Rooms;

public static class RoomErrors
{
    public static readonly Error InvalidName =
        Error.Validation("Room.InvalidName", "Room name is required.");

    public static readonly Error InvalidCapacity =
        Error.Validation("Room.InvalidCapacity", "Capacity must be strictly positive.");

    public static readonly Error NotFound =
        Error.NotFound("Room.NotFound", "The room does not exist.");
}
