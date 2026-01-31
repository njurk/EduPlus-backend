using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API.Helpers
{
    public static class JwtHelper
    {
        public static int? ValidateTokenAndGetUserId(string? token, IConfiguration config)
        {
            if (string.IsNullOrEmpty(token)) return null;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(config["Jwt:Key"]!);
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                }, out var validated);
                var claim = ((JwtSecurityToken)validated).Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;
                return int.TryParse(claim, out var id) ? id : null;
            }
            catch { return null; }
        }
    }
}
