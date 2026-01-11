using System.Linq;
using System.Security.Claims;
using VsaSample.Application.Abstractions.Authentication;
using VsaSample.Domain.Enums;

namespace VsaSample.Api.Authentication;

public sealed class OrganizationContext(IHttpContextAccessor httpContextAccessor) : IOrganizationContext
{
    public Guid OrganizationId => GetGuidClaim("org_id");
    public Guid UserId => GetGuidClaim("sub");
    public bool IsSuperAdmin => GetRoles()
        .Any(role => string.Equals(role, UserRole.SuperAdmin.ToString(), StringComparison.OrdinalIgnoreCase));

    private Guid GetGuidClaim(string claimType)
    {
        var principal = httpContextAccessor.HttpContext?.User
            ?? throw new UnauthorizedAccessException("User context is unavailable.");

        var value = principal.FindFirstValue(claimType);
        if (!Guid.TryParse(value, out var parsed))
        {
            throw new UnauthorizedAccessException($"Missing or invalid '{claimType}' claim.");
        }

        return parsed;
    }

    private IEnumerable<string> GetRoles()
    {
        var principal = httpContextAccessor.HttpContext?.User;
        if (principal is null)
        {
            return Array.Empty<string>();
        }

        var roleClaims = principal.FindAll("role").Select(c => c.Value).ToList();
        if (roleClaims.Count == 0)
        {
            roleClaims = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        }

        return roleClaims;
    }
}
