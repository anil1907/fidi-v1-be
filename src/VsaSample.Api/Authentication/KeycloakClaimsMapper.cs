using System.Security.Claims;
using System.Text.Json;

namespace VsaSample.Api.Authentication;

internal static class KeycloakClaimsMapper
{
    public static void Map(ClaimsIdentity identity)
    {
        MapRealmRoles(identity);
        MapNameIdentifier(identity);
        MapPreferredUsername(identity);
    }

    private static void MapRealmRoles(ClaimsIdentity identity)
    {
        var realmAccess = identity.FindFirst("realm_access")?.Value;
        if (string.IsNullOrWhiteSpace(realmAccess))
        {
            return;
        }

        using var json = JsonDocument.Parse(realmAccess);
        if (!json.RootElement.TryGetProperty("roles", out var rolesElement) ||
            rolesElement.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        foreach (var role in rolesElement.EnumerateArray())
        {
            var value = role.GetString();
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            AddClaimIfMissing(identity, "role", value);
            AddClaimIfMissing(identity, ClaimTypes.Role, value);
        }
    }

    private static void MapNameIdentifier(ClaimsIdentity identity)
    {
        var subject = identity.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(subject))
        {
            return;
        }

        AddClaimIfMissing(identity, ClaimTypes.NameIdentifier, subject);
    }

    private static void MapPreferredUsername(ClaimsIdentity identity)
    {
        var preferredUsername = identity.FindFirst("preferred_username")?.Value;
        if (string.IsNullOrWhiteSpace(preferredUsername))
        {
            return;
        }

        AddClaimIfMissing(identity, "Username", preferredUsername);
    }

    private static void AddClaimIfMissing(ClaimsIdentity identity, string type, string value)
    {
        if (identity.HasClaim(type, value))
        {
            return;
        }

        identity.AddClaim(new Claim(type, value));
    }
}
