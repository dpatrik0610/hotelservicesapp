using JWT.Algorithms;
using JWT.Builder;

namespace Hotelservices.UserAuth.Helpers {
    public class JwtTokenGenerator
    {
        private readonly string _secretKey;

        public JwtTokenGenerator(string secretKey)
        {
            _secretKey = secretKey;
        }

        public string GenerateJwtToken(string userId, string username, List<string> roles)
        {
            var currentTime = DateTime.UtcNow;
            var expireTime = currentTime.AddHours(1); // Token expires in 1 hour
            var jwtId = Guid.NewGuid().ToString();

            var payload = new Dictionary<string, object>
            {
                { "sub", userId },                                             // Subject (User ID)
                { "exp", new DateTimeOffset(expireTime).ToUnixTimeSeconds() }, // Expiration Time
                { "jti", jwtId },                                              // JWT ID
                { "username", username },                                      // Username
                { "roles", roles }                                             // Roles
            };

            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_secretKey)
                .AddClaim("payload", payload)
                .Encode();

            return token;
        }
    }
}
