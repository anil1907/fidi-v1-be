namespace VsaSample.Application.Abstractions.Authentication;

public interface IUserContext
{
    long UserId { get; }
    string Username { get; }
}