namespace VsaSample.Application.Abstractions.Caching;

public interface ICacheService
{
    ValueTask<T?> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T?>> factory,
        CancellationToken cancellationToken = default);
    ValueTask SetAsync<T>(string key, T value, CancellationToken cancellationToken = default);
    ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default);
}
