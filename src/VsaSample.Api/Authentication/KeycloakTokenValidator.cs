using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace VsaSample.Api.Authentication;

public sealed class KeycloakTokenValidator(IOptionsMonitor<JwtBearerOptions> optionsMonitor)
{
    private readonly IOptionsMonitor<JwtBearerOptions> _optionsMonitor = optionsMonitor;

    public async Task<ClaimsPrincipal?> ValidateAccessTokenAsync(string token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        try
        {
            var options = _optionsMonitor.Get(AuthConstants.KeycloakScheme);
            var parameters = options.TokenValidationParameters.Clone();
            if (options.ConfigurationManager is not null)
            {
                var config = await options.ConfigurationManager.GetConfigurationAsync(cancellationToken);
                parameters.ValidIssuer ??= config.Issuer;
                parameters.IssuerSigningKeys = config.SigningKeys;
            }

            var handler = new JwtSecurityTokenHandler
            {
                MapInboundClaims = false
            };

            var principal = handler.ValidateToken(token, parameters, out _);
            if (principal.Identity is ClaimsIdentity identity)
            {
                KeycloakClaimsMapper.Map(identity);
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
