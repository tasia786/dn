using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Common.Abstractions;

public interface ICommandHandler<in TCommand>
{
    Task<Result> HandleAsync(TCommand command, CancellationToken ct = default);
}

public interface ICommandHandler<in TCommand, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken ct = default);
}
