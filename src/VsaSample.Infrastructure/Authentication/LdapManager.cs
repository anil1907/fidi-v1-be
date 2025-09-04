using Novell.Directory.Ldap;

namespace VsaSample.Infrastructure.Authentication;

internal sealed class LdapManager : ILdapManager
{
    private readonly LdapOptions _options;

    public LdapManager(IOptions<LdapOptions> options)
    {
        _options = options.Value;
    }

    public Task<bool> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = new LdapConnection();
            connection.Connect(_options.Host, _options.Port);
            var userDn = string.IsNullOrWhiteSpace(_options.Domain)
                ? username
                : $"{_options.Domain}\\{username}";
            connection.Bind(userDn, password);
            connection.Disconnect();
            return Task.FromResult(true);
        }
        catch (LdapException)
        {
            return Task.FromResult(false);
        }
    }
}
