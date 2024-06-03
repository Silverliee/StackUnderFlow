using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace StackUnderFlow.Application.Middleware;

public class AuthentificationMiddleware
{
    private readonly IConfiguration _configuration;

    public AuthentificationMiddleware(IConfiguration configuration)
    {
        _configuration = configuration;
        
    }
    
    public string GenerateJwtToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = _configuration["Jwt:Key"];
        var _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}