using VsaSample.Application.Features.Categories;

namespace VsaSample.Infrastructure.Database;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ICacheService _cache;
    private readonly DbSet<Category> _set;
    private readonly TagTemplateKeys _tags;
    private readonly CacheKeys _cacheKeys;

    public CategoryRepository(ApplicationDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
        _set = db.Set<Category>();
        _tags = CategoriesFeature.Instance.TagTemplates;
        _cacheKeys = CategoriesFeature.Instance.Cache;
    }

    public IQueryable<Category> Query => _set.AsNoTracking();

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = _tags.ById.Replace("{Id}", id.ToString());
        return _cache.GetOrCreateAsync(
                cacheKey,
                ct => new ValueTask<Category?>(_set.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct)),
                cancellationToken)
            .AsTask();
    }

    public async Task<Guid> AddAsync(Category entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.SetAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), entity, cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        _set.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.SetAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), entity, cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
    }

    public async Task DeleteAsync(Category entity, CancellationToken cancellationToken = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
    }
}
