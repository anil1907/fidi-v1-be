namespace VsaSample.Infrastructure.Options;

public sealed class LdapOptions
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Domain { get; set; } = string.Empty;
}
