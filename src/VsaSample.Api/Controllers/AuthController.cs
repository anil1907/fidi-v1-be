using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VsaSample.Api.Authentication;
using VsaSample.Api.Contracts.Auth;
using VsaSample.Api.Options;

namespace VsaSample.Api.Controllers;

[ApiController]
[ApiVersionNeutral]
[Route("auth")]
public sealed class AuthController(
    KeycloakTokenClient tokenClient,
    KeycloakTokenValidator tokenValidator,
    IOptions<KeycloakOptions> keycloakOptions,
    IOptions<SpaOptions> spaOptions,
    ILogger<AuthController> logger)
    : ControllerBase
{
    private readonly KeycloakTokenClient _tokenClient = tokenClient;
    private readonly KeycloakTokenValidator _tokenValidator = tokenValidator;
    private readonly KeycloakOptions _keycloakOptions = keycloakOptions.Value;
    private readonly SpaOptions _spaOptions = spaOptions.Value;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("exchange")]
    public async Task<IActionResult> Exchange([FromBody] ExchangeRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Code) ||
            string.IsNullOrWhiteSpace(request.CodeVerifier) ||
            string.IsNullOrWhiteSpace(request.RedirectUri))
        {
            return BadRequest(new { ok = false });
        }

        TokenResponse tokenResponse;
        try
        {
            tokenResponse = await _tokenClient.ExchangeAuthorizationCodeAsync(request, cancellationToken);
        }
        catch (KeycloakTokenExchangeException ex)
        {
            _logger.LogWarning(ex, "Keycloak token exchange failed with status {StatusCode}.", ex.StatusCode);
            return StatusCode((int)ex.StatusCode, new { ok = false });
        }

        AppendTokenCookie(AuthConstants.AccessTokenCookieName, tokenResponse.AccessToken);
        AppendTokenCookie(AuthConstants.RefreshTokenCookieName, tokenResponse.RefreshToken);
        AppendTokenCookie(AuthConstants.IdTokenCookieName, tokenResponse.IdToken);

        return Ok(new { ok = true });
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var token = GetAccessToken();
        if (string.IsNullOrWhiteSpace(token))
        {
            return Unauthorized();
        }

        var principal = await _tokenValidator.ValidateAccessTokenAsync(token, cancellationToken);
        if (principal is null)
        {
            return Unauthorized();
        }

        var response = new MeResponse(
            Sub: principal.FindFirstValue("sub") ?? principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
            Email: principal.FindFirstValue("email"),
            OrgId: principal.FindFirstValue("org_id"),
            Roles: GetRoles(principal));

        return Ok(response);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var idToken = Request.Cookies[AuthConstants.IdTokenCookieName];

        ClearAuthCookies();

        var logoutUrl = BuildLogoutUrl(idToken);
        return Ok(new { ok = true, logoutUrl });
    }

    private string? GetAccessToken()
    {
        var headerToken = GetBearerTokenFromHeader();
        if (!string.IsNullOrWhiteSpace(headerToken))
        {
            return headerToken;
        }

        return Request.Cookies.TryGetValue(AuthConstants.AccessTokenCookieName, out var cookieToken)
            ? cookieToken
            : null;
    }

    private string? GetBearerTokenFromHeader()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorization))
        {
            return null;
        }

        var header = authorization.ToString();
        if (!header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return header["Bearer ".Length..].Trim();
    }

    private static IReadOnlyCollection<string> GetRoles(ClaimsPrincipal principal)
    {
        var roles = principal.FindAll("role").Select(c => c.Value)
            .Concat(principal.FindAll(ClaimTypes.Role).Select(c => c.Value))
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return roles;
    }

    private static CookieOptions BuildCookieOptions() =>
        new()
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Path = "/"
        };

    private void AppendTokenCookie(string name, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        Response.Cookies.Append(name, value, BuildCookieOptions());
    }

    private void ClearAuthCookies()
    {
        var options = BuildCookieOptions();
        Response.Cookies.Delete(AuthConstants.AccessTokenCookieName, options);
        Response.Cookies.Delete(AuthConstants.RefreshTokenCookieName, options);
        Response.Cookies.Delete(AuthConstants.IdTokenCookieName, options);
    }

    private string? BuildLogoutUrl(string? idToken)
    {
        if (string.IsNullOrWhiteSpace(idToken))
        {
            return null;
        }

        var baseUrl = _keycloakOptions.BaseUrl.TrimEnd('/');
        var realm = _keycloakOptions.Realm;
        var spaOrigin = _spaOptions.Origin.TrimEnd('/');

        if (string.IsNullOrWhiteSpace(baseUrl) ||
            string.IsNullOrWhiteSpace(realm) ||
            string.IsNullOrWhiteSpace(spaOrigin))
        {
            return null;
        }

        var postLogoutRedirectUri = $"{spaOrigin}/";
        var query =
            $"id_token_hint={Uri.EscapeDataString(idToken)}&post_logout_redirect_uri={Uri.EscapeDataString(postLogoutRedirectUri)}";

        return $"{baseUrl}/realms/{realm}/protocol/openid-connect/logout?{query}";
    }
}
