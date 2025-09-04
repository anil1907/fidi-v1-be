namespace VsaSample.Application.Features.SubCategories.Get;

internal sealed class GetSubCategoriesPagedQueryHandler(
    IApplicationDbContext db,
    ISieveProcessor sieve,
    ICacheService cache,
    ILogger<GetSubCategoriesPagedQueryHandler> logger)
    : IQueryHandler<GetSubCategoriesPagedQuery, PagedResult<SubCategoryResponse>>
{
    public async Task<Result<PagedResult<SubCategoryResponse>>> Handle(GetSubCategoriesPagedQuery query, CancellationToken cancellationToken)
    {
        var cachePrefix = SubCategoriesFeature.Instance.Cache.PagePrefix;
        var cacheKey = $"{cachePrefix}:{query.Sieve.Page}-{query.Sieve.PageSize}-{query.Sieve.Filters}-{query.Sieve.Sorts}";
        var pageSize = query.Sieve.PageSize ?? 10;
        var pageNumber = query.Sieve.Page ?? 1;

        var payload = await cache.GetOrCreateAsync(
            cacheKey,
            async ct =>
            {
                var baseQuery = db.SubCategories.AsNoTracking();
                var filtered = sieve.Apply(query.Sieve, baseQuery, applyPagination: false);
                var total = await filtered.CountAsync(ct);

                var paged = sieve.Apply(query.Sieve, filtered, applyPagination: true);
                var items = await paged
                    .Select(e => e.ToResponse())
                    .ToListAsync(ct);

                return PagedResult<SubCategoryResponse>.Create(items, pageNumber, pageSize, total);
            },
            cancellationToken);

        logger.LogInformation(
            "Fetched sub categories page {Page}/{Size} (Total: {Total})",
            pageNumber,
            pageSize,
            payload?.TotalCount);

        return Result<PagedResult<SubCategoryResponse>>.Success(payload!);
    }
}

