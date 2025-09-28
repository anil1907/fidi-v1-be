namespace VsaSample.Application.Features.Clients.Update;

public sealed record UpdateClientCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string? Notes,
    IReadOnlyCollection<string>? Goals,
    bool? IsActive) : ICommand
{
    public string Culture { get; init; } = Cultures.Default;
}

public sealed record UpdateClientRequest(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string? Notes,
    IReadOnlyCollection<string>? Goals,
    bool? IsActive);
