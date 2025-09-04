namespace VsaSample.Application.Features.Users.Login;

public sealed record LoginUserCommand(string Username, string Password) : ICommand<LoginUserResponse>
{
    public string Culture { get; init; } = Cultures.Default;
}
