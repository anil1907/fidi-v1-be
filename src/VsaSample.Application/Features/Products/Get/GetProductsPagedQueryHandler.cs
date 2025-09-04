namespace VsaSample.Application.Features.Products.Get;

public sealed class GetProductsPagedQueryHandler(
    IProductRepository repository,
    ISieveProcessor sieve,
    ILogger<GetProductsPagedQueryHandler> logger)
    : IQueryHandler<GetProductsPagedQuery, PagedResult<ProductResponse>>
{
    public async Task<Result<PagedResult<ProductResponse>>> Handle(GetProductsPagedQuery query, CancellationToken cancellationToken)
    {
        var baseQuery = repository.Query.Include(x => x.Translations);
        var filtered = sieve.Apply(query.Sieve, baseQuery, applyPagination: false);
        var total = await filtered.CountAsync(cancellationToken);

        var paged = sieve.Apply(query.Sieve, filtered, applyPagination: true);
        var items = await paged
            .Select(e => e.ToResponse(query.Culture, query.DefaultCulture))
            .ToListAsync(cancellationToken);

        var pageSize = query.Sieve.PageSize ?? 10;
        var pageNumber = query.Sieve.Page ?? 1;
        var payload = PagedResult<ProductResponse>.Create(items, pageNumber, pageSize, total);

        logger.LogInformation(
            "Fetched products page {Page}/{Size} (Total: {Total}) for culture {Culture}/{Default}",
            pageNumber,
            pageSize,
            total,
            query.Culture,
            query.DefaultCulture);

        return Result<PagedResult<ProductResponse>>.Success(payload);
    }
}
