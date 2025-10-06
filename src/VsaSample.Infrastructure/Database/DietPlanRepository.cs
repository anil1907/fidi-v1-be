using VsaSample.Application.Features.DietPlans;

namespace VsaSample.Infrastructure.Database;

public sealed class DietPlanRepository : IDietPlanRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ICacheService _cache;
    private readonly DbSet<DietPlan> _set;
    private readonly TagTemplateKeys _tags;
    private readonly CacheKeys _cacheKeys;

    public DietPlanRepository(ApplicationDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
        _set = db.Set<DietPlan>();
        _tags = DietPlansFeature.Instance.TagTemplates;
        _cacheKeys = DietPlansFeature.Instance.Cache;
    }

    public IQueryable<DietPlan> Query => _set.AsNoTracking();

    public Task<DietPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = _tags.ById.Replace("{Id}", id.ToString());
        return _cache.GetOrCreateAsync(
                cacheKey,
                ct => new ValueTask<DietPlan?>(_set.AsNoTracking().FirstOrDefaultAsync(plan => plan.Id == id, ct)),
                cancellationToken)
            .AsTask();
    }

    public Task<DietPlan?> GetEntityByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _set.FirstOrDefaultAsync(plan => plan.Id == id, cancellationToken);

    public async Task<Guid> AddAsync(DietPlan entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.SetAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), entity, cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(DietPlan entity, CancellationToken cancellationToken = default)
    {
        _set.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.SetAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), entity, cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
    }

    public async Task DeleteAsync(DietPlan entity, CancellationToken cancellationToken = default)
    {
        _set.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(_tags.ById.Replace("{Id}", entity.Id.ToString()), cancellationToken);
        await _cache.RemoveAsync(_cacheKeys.ListTag, cancellationToken);
    }
}
