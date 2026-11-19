using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class RoomRepository(RoomHubDbContext db) : IRoomRepository
{
    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await db.Rooms.FindAsync([id], ct);

    public void Add(Room room) => db.Rooms.Add(room);
}
