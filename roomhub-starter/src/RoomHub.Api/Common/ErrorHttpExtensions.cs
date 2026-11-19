using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Common;

public static class ErrorHttpExtensions
{
    // Maps a domain Error to a ProblemDetails response (application/problem+json).
    public static IResult ToProblem(this Error error) =>
        Results.Problem(
            title: error.Code,
            detail: error.Message,
            statusCode: error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            });
}
