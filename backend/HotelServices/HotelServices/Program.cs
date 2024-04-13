using HotelServices.Shared.Database;
using HotelServices.Services.Interfaces;
using HotelServices.Services;

using Serilog;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetValue<string>("MongoDB:ConnectionURI");

if (string.IsNullOrEmpty(connectionString))
{
    throw new ApplicationException("MongoDB connection settings are missing or invalid.");
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddSingleton<IMongoDatabaseProvider>(provider => {
    try
    {
        return new MongoDatabaseProvider(connectionString);
    }
    catch (Exception ex)
    {
        throw new ApplicationException("Failed to establish connection to the MongoDB server.", ex);
    }
});

// Adding Services
builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserReservationService, UserReservationService>();

// Adding auth
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();

// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins"); // Configure CORS middleware

app.UseAuthorization();

app.MapControllers();

app.Run();
