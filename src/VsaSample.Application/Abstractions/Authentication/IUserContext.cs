namespace VsaSample.Application.Abstractions.Authentication;

public interface IUserContext
{
    Guid UserId { get; }
    string Username { get; }
}
