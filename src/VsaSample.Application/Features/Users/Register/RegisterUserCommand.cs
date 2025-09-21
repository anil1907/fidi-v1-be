namespace VsaSample.Application.Features.Users.Register;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Username,
    string Password,
    UserRole Role) : ICommand<long>
{
    public string Culture { get; init; } = Cultures.Default;
}
