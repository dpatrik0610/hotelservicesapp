using HotelServices.Shared.Database;
using HotelServices.Services.Interfaces;
using HotelServices.Services;

using Serilog;
using HotelServices.Shared.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("../HotelServices.Shared/Configurations/configurations.json", optional: false, reloadOnChange: true);
var configuration = builder.Configuration;

// Add logging services
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Adding MongoDB to the services
var connectionString = configuration.GetConnectionString("MongoDBConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new ApplicationException("MongoDB connection settings are missing or invalid.");
}

// Adding Services
builder.Services.AddSingleton<IMongoDatabaseProvider>(provider =>
{
    return new MongoDatabaseProvider(connectionString);
});

builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserReservationService, UserReservationService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    CookieConfiguration.ConfigureAuthenticationCookie(options.Cookie);
});

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

builder.Services.AddControllers();



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
