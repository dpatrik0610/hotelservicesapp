using HotelServices.Database;
using HotelServices.Services.Interfaces;
using HotelServices.Services;

using Serilog;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetValue<string>("MongoDB:ConnectionURI");
var databaseName = builder.Configuration.GetValue<string>("MongoDB:DatabaseName");

if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
{
    throw new ApplicationException("MongoDB connection settings are missing or invalid.");
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddSingleton<IMongoDatabaseProvider>(provider => {
    try
    {
        return new MongoDatabaseProvider(connectionString, databaseName, provider.GetRequiredService<ILogger<MongoDatabaseProvider>>());
    }
    catch (Exception ex)
    {
        throw new ApplicationException("Failed to establish connection to the MongoDB server.", ex);
    }
});

builder.Services.AddSingleton<IRoomService, RoomService>();
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
