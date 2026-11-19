using RoomHub.Api.Common.Abstractions;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed class GetRoomHandler(IRoomRepository rooms) : IQueryHandler<GetRoomQuery, GetRoomResponse>
{
    public async Task<Result<GetRoomResponse>> HandleAsync(GetRoomQuery query, CancellationToken ct = default)
    {
        var room = await rooms.GetByIdAsync(query.Id, ct);
        if (room is null)
            return RoomErrors.NotFound;

        return new GetRoomResponse(room.Id, room.Name, room.Capacity);
    }
}
