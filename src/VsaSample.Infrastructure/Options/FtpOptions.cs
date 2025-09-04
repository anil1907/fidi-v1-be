namespace VsaSample.Infrastructure.Options;

public sealed class FtpOptions
{
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; } = 21;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
