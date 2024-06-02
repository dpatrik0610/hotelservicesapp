using HotelServices.Shared.Database;
using HotelServices.Services.Interfaces;
using HotelServices.Services;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json.Linq;

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

// Adding Services
builder.Services.AddSingleton<IMongoDatabaseProvider>(provider =>
{
    var connectionString = configuration.GetConnectionString("MongoDBConnection");
    return new MongoDatabaseProvider(connectionString);
});

builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserReservationService, UserReservationService>();

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

// Configure JWT authentication
var authServerUrl = configuration["AuthServer:Url"];
var audience = configuration["Jwt:Audience"];
var secretKey = configuration["Jwt:Secret"];
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

// Configure CORS
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

// Add Controllers
builder.Services.AddControllers();

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Development Environment Setup
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware Setup
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
Log.Information("The Hotel-API is online.");
app.Run();

