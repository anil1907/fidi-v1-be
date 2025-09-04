namespace VsaSample.Application.Features.Categories.Get;

internal sealed class GetCategoriesPagedQueryHandler(
    ICategoryRepository repository,
    ISieveProcessor sieve,
    ILogger<GetCategoriesPagedQueryHandler> logger)
    : IQueryHandler<GetCategoriesPagedQuery, PagedResult<CategoryResponse>>
{
    public async Task<Result<PagedResult<CategoryResponse>>> Handle(GetCategoriesPagedQuery query, CancellationToken cancellationToken)
    {
        var baseQuery = repository.Query;
        var filtered = sieve.Apply(query.Sieve, baseQuery, applyPagination: false);
        var total = await filtered.CountAsync(cancellationToken);

        var paged = sieve.Apply(query.Sieve, filtered, applyPagination: true);
        var items = await paged
            .Select(e => e.ToResponse())
            .ToListAsync(cancellationToken);

        var pageSize = query.Sieve.PageSize ?? 10;
        var pageNumber = query.Sieve.Page ?? 1;
        var payload = PagedResult<CategoryResponse>.Create(items, pageNumber, pageSize, total);

        logger.LogInformation(
            "Fetched categories page {Page}/{Size} (Total: {Total})",
            pageNumber,
            pageSize,
            total);

        return Result<PagedResult<CategoryResponse>>.Success(payload);
    }
}
