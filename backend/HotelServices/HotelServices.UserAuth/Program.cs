using HotelServices.Shared.Database;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Hotelservices.UserAuth.Helpers;
using Hotelservices.UserAuth.IdentityModels;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
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

var databaseName = "Hotel";

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

// Adding MongoDB Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, ObjectId>(connectionString, databaseName);

// builder.Services.AddSingleton<JwtSecurityTokenHandler>();
var secretKey = configuration["Jwt:Secret"];
builder.Services.AddSingleton(new JwtTokenGenerator(secretKey));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
