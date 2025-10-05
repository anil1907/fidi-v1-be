using VsaSample.Application.Features.Templates;

namespace VsaSample.Infrastructure.Database;

public sealed class TemplateRepository : ITemplateRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ICacheService _cache;
    private readonly DbSet<Template> _set;
    private readonly TagTemplateKeys _tags;
    private readonly CacheKeys _cacheKeys;

    public TemplateRepository(ApplicationDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
        _set = db.Set<Template>();
        _tags = TemplatesFeature.Instance.TagTemplates;
        _cacheKeys = TemplatesFeature.Instance.Cache;
    }

    public IQueryable<Template> Query => _set.AsNoTracking();

    public Task<Template?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = _tags.ById.Replace("{Id}", id.ToString());
        return _cache.GetOrCreateAsync(
                cacheKey,
                ct => new ValueTask<Template?>(_set.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct)),
                cancellationToken)
            .AsTask();
    }

    public Task<Template?> GetEntityByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _set.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

    public async Task<Guid> AddAsync(Template entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.SetAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), entity, cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(Template entity, CancellationToken cancellationToken = default)
    {
        _set.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.SetAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), entity, cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
    }

    public async Task DeleteAsync(Template entity, CancellationToken cancellationToken = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
    }
}
