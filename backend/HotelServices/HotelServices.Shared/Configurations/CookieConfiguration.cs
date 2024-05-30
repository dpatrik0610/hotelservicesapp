using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;


namespace HotelServices.Shared.Configurations
{
    public static class CookieConfiguration
    {
        public static void ConfigureAuthenticationCookie(CookieBuilder options)
        {
            var configJson = JObject.Parse(File.ReadAllText("cookieSettings.json"));

            options.Name = (string)configJson["Name"];
            options.Domain = (string)configJson["Domain"];
            options.Path = (string)configJson["Path"];
            options.HttpOnly = (bool)configJson["HttpOnly"];
            options.SecurePolicy = Enum.Parse<CookieSecurePolicy>((string)configJson["SecurePolicy"]);
            options.SameSite = Enum.Parse<SameSiteMode>((string)configJson["SameSite"]);
        }
    }
}
