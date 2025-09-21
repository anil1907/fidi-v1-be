using Microsoft.IdentityModel.JsonWebTokens;

namespace VsaSample.Infrastructure.Authentication;

public class TokenProvider(IOptions<JwtOptions> options) : ITokenProvider
{
    private readonly JwtOptions _options = options.Value;

    public string Create(User user)
    {
        var secretKey = _options.Secret;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.PreferredUsername, user.Username)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_options.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Claims = GetUserClaims(user)
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }
    
    private static IDictionary<string, object> GetUserClaims(User user) =>
        new Dictionary<string, object>
        {
            [ClaimTypes.NameIdentifier] = user.Id.ToString(),
            [ClaimTypes.Role] = user.Role.ToString(),
            [ClaimTypes.Name] = user.FirstName,
            [ClaimTypes.Surname] = user.LastName,
            [ClaimTypes.Email] = user.Email,
            ["Username"] = user.Username
        };
}
