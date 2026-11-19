using RoomHub.Api.Common;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoom;
using RoomHub.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Shared in-memory storage
builder.Services.AddSingleton<RoomRepository>();
builder.Services.AddSingleton<BookingRepository>();

// One handler per feature slice
builder.Services.AddScoped<CreateRoomHandler>();
builder.Services.AddScoped<GetRoomHandler>();
builder.Services.AddScoped<CreateBookingHandler>();
builder.Services.AddScoped<GetBookingHandler>();
builder.Services.AddScoped<CancelBookingHandler>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapGet("/", () => "RoomHub API is running.");
app.MapCreateRoom();
app.MapGetRoom();
app.MapCreateBooking();
app.MapGetBooking();
app.MapCancelBooking();

app.Run();
