using VsaSample.Application.Features.Products;

namespace VsaSample.Infrastructure.Database;

public sealed class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ICacheService _cache;
    private readonly DbSet<Product> _set;
    private readonly TagTemplateKeys _tags;
    private readonly CacheKeys _cacheKeys;

    public ProductRepository(ApplicationDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
        _set = db.Set<Product>();
        _tags = ProductsFeature.Instance.TagTemplates;
        _cacheKeys = ProductsFeature.Instance.Cache;
    }

    public IQueryable<Product> Query => _set.AsNoTracking();

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = _tags.ById.Replace("{Id}", id.ToString());
        return _cache.GetOrCreateAsync(
                cacheKey,
                async ct => await _db.Products
                    .AsNoTracking()
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(x => x.Id == id, ct),
                cancellationToken)
            .AsTask();
    }

    public async Task<Guid> AddAsync(Product entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.SetAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), entity, cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(Product entity, CancellationToken cancellationToken = default)
    {
        _set.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.SetAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), entity, cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
    }

    public async Task DeleteAsync(Product entity, CancellationToken cancellationToken = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
    }
}
