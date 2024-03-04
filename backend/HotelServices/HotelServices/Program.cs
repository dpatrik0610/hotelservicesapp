using HotelServices.Database;
using HotelServices.Services.Interfaces;
using HotelServices.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoDatabaseProvider>(provider => { 
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetValue<string>("MongoDB:ConnectionURI");
    var databaseName = config.GetValue<string>("MongoDB:DatabaseName");

    if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
    {
        // Log or throw an exception indicating missing or invalid configuration.
        throw new ApplicationException("MongoDB connection settings are missing or invalid.");
    }

    try
    {
        return new MongoDatabaseProvider(connectionString, databaseName);
    }
    catch (Exception ex)
    {
        // Log or throw an exception indicating failure to establish connection.
        throw new ApplicationException("Failed to establish connection to the MongoDB server.", ex);
    }
});

builder.Services.AddSingleton<IRoomService, RoomService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
