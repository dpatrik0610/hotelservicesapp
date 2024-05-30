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

builder.Services.AddSingleton<IMongoDatabaseProvider>(provider =>
{
    const int maxRetryAttempts = 3;
    const int delayBetweenRetriesMs = 2000;
    int retryAttempt = 0;

    while (true)
    {
        try
        {
            return new MongoDatabaseProvider(connectionString);
        }
        catch (Exception ex)
        {
            retryAttempt++;
            if (retryAttempt > maxRetryAttempts)
            {
                throw new ApplicationException("Failed to establish connection to the MongoDB server after multiple retries.", ex);
            }

            // Log the retry attempt
            Console.WriteLine($"Retry attempt {retryAttempt} failed. Retrying in {delayBetweenRetriesMs / 1000} seconds.");

            // Wait before retrying
            Thread.Sleep(delayBetweenRetriesMs);
        }
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
