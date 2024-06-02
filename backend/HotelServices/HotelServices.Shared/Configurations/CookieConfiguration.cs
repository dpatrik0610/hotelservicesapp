using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

public class CookieConfiguration
{
    public CookieOptions GetCookieOptions(JObject configJson)
    {
        var cookieOptions = new CookieOptions
        {
            Domain = configJson["Domain"]?.ToString(),
            Path = configJson["Path"]?.ToString(),
            HttpOnly = configJson["HttpOnly"] != null ? (bool)configJson["HttpOnly"] : false,
            Secure = configJson["Secure"] != null ? (bool)configJson["Secure"] : false,
            SameSite = configJson["SameSite"] != null ? Enum.Parse<SameSiteMode>(configJson["SameSite"].ToString()) : SameSiteMode.Lax
        };

        return cookieOptions;
    }
}
