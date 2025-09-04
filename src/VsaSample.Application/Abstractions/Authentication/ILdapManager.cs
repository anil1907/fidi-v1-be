namespace VsaSample.Application.Abstractions.Authentication;

public interface ILdapManager
{
    Task<bool> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
}
