using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Rooms;

public sealed class Room
{
    // Used by EF Core
    private Room() { }

    private Room(Guid id, string name, int capacity)
    {
        Id = id;
        Name = name;
        Capacity = capacity;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }

    // R1
    public static Result<Room> Create(string? name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoomErrors.InvalidName;

        if (capacity <= 0)
            return RoomErrors.InvalidCapacity;

        return new Room(Guid.NewGuid(), name.Trim(), capacity);
    }

    public bool CanHost(int attendees) => attendees <= Capacity;
}
