using HotelServices.Shared.Database;
using Hotelservices.UserAuth.Helpers;
using Hotelservices.UserAuth.IdentityModels;
using Serilog;
using Newtonsoft.Json.Linq;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Setting up Configurations
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = builder.Configuration;
var cookieConfig = JObject.Parse(File.ReadAllText("../HotelServices.Shared/Configurations/cookieSettings.json"));

// Add logging services
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// Adding MongoDB to the services
var databaseName = configuration["DatabaseName"];

var connectionString = configuration.GetConnectionString("MongoDBConnection");
builder.Services.AddScoped<IMongoDatabaseProvider>(provider =>
{
    return new MongoDatabaseProvider(connectionString);
});

// Adding MongoDB Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, ObjectId>(connectionString, databaseName);

var secretKey = configuration["Jwt:Secret"];
builder.Services.AddSingleton(new JwtTokenGenerator(secretKey));

// Configure CookieOptions
builder.Services.Configure<CookieOptions>(options =>
{
    var cookieConfiguration = new CookieConfiguration();
    var configuredOptions = cookieConfiguration.GetCookieOptions(cookieConfig);
    options.Domain = configuredOptions.Domain;
    options.Path = configuredOptions.Path;
    options.HttpOnly = configuredOptions.HttpOnly;
    options.Secure = configuredOptions.Secure;
    options.SameSite = configuredOptions.SameSite;
});

// Add Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JWT authentication
var authServerUrl = configuration["AuthServer:Url"];
var audience = configuration["Jwt:Audience"];
var issuer = configuration["Jwt:Issuer"];
var requireHttpsMetadata = bool.Parse(configuration["Jwt:RequireHttpsMetadata"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = authServerUrl;
    options.Audience = audience;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
    options.RequireHttpsMetadata = requireHttpsMetadata;
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Development Environment Setup
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware Setup
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
