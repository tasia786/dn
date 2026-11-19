using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Common.Abstractions;
using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoom;
using RoomHub.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// JSON: camelCase (default) + enums as text
builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Errors not produced by handlers (e.g. malformed JSON) also become ProblemDetails
builder.Services.AddProblemDetails();

// "Now" comes from TimeProvider (R5)
builder.Services.AddSingleton(TimeProvider.System);

// Persistence: EF Core + SQLite, file stored next to the project so data survives restarts
var connectionString = builder.Configuration.GetConnectionString("RoomHub")
    ?? $"Data Source={Path.Combine(builder.Environment.ContentRootPath, "roomhub.db")}";
builder.Services.AddDbContext<RoomHubDbContext>(o => o.UseSqlite(connectionString));
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Handlers (one per slice)
builder.Services.AddScoped<ICommandHandler<CreateRoomCommand, CreateRoomResponse>, CreateRoomHandler>();
builder.Services.AddScoped<IQueryHandler<GetRoomQuery, GetRoomResponse>, GetRoomHandler>();
builder.Services.AddScoped<ICommandHandler<CreateBookingCommand, CreateBookingResponse>, CreateBookingHandler>();
builder.Services.AddScoped<IQueryHandler<GetBookingQuery, GetBookingResponse>, GetBookingHandler>();
builder.Services.AddScoped<ICommandHandler<CancelBookingCommand>, CancelBookingHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<RoomHubDbContext>().Database.EnsureCreated();
}

// Malformed request bodies -> 400 ProblemDetails (not 500)
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    StatusCodeSelector = ex => ex is BadHttpRequestException bad
        ? bad.StatusCode
        : StatusCodes.Status500InternalServerError
});
app.UseStatusCodePages();

app.MapGet("/", () => "RoomHub API is running.");
app.MapCreateRoom();
app.MapGetRoom();
app.MapCreateBooking();
app.MapGetBooking();
app.MapCancelBooking();

app.Run();
