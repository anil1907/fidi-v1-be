namespace VsaSample.Infrastructure.Options;

public sealed class CorsOptions
{
    public IReadOnlyCollection<string> AllowOrigins { get; init; } = new List<string>();
}

