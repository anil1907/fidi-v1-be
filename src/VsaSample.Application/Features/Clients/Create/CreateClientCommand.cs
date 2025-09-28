namespace VsaSample.Application.Features.Clients.Create;

public sealed record CreateClientCommand(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string? Notes,
    IReadOnlyCollection<string>? Goals) : ICommand<Guid>
{
    public string Culture { get; init; } = Cultures.Default;
}
