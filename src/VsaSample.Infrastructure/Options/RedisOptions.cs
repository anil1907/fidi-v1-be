namespace VsaSample.Infrastructure.Options;

public sealed class RedisOptions
{
    public string Password { get; init; } = null!;
    public bool Ssl { get; init; }
    public int ConnectTimeout { get; init; }
    public int ConnectRetry { get; init; }
    public int Database { get; init; }
    public IReadOnlyCollection<RedisHostOptions> Hosts { get; init; } = new List<RedisHostOptions>();
}

public sealed class RedisHostOptions
{
    public string Host { get; init; } = null!;
    public int Port { get; init; }
}
