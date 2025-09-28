namespace VsaSample.Application.Features.Clients.Shared;

public sealed record ClientResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public IReadOnlyList<string> Goals { get; init; } = Array.Empty<string>();
    public bool IsActive { get; init; }
    public DateTime? CreateDate { get; init; }
}
