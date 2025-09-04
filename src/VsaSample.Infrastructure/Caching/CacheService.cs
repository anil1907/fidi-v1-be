using Microsoft.Extensions.Caching.Hybrid;

namespace VsaSample.Infrastructure.Caching;

public sealed class CacheService(HybridCache cache) : ICacheService
{
    public ValueTask<T?> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T?>> factory,
        CancellationToken cancellationToken = default)
        => cache.GetOrCreateAsync(key, factory, cancellationToken: cancellationToken);

    public ValueTask SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        => cache.SetAsync(key, value!, cancellationToken: cancellationToken);

    public ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default)
        => cache.RemoveAsync(key, cancellationToken);
}
