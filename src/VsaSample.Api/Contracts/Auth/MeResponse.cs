namespace VsaSample.Api.Contracts.Auth;

public sealed record MeResponse(
    [property: JsonPropertyName("sub")] string Sub,
    [property: JsonPropertyName("email")] string? Email,
    [property: JsonPropertyName("org_id")] string? OrgId,
    [property: JsonPropertyName("roles")] IReadOnlyCollection<string> Roles);
