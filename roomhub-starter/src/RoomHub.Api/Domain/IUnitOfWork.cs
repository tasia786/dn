namespace RoomHub.Api.Domain;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct = default);
}
