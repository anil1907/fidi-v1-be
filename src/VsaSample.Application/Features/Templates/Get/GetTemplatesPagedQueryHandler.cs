namespace VsaSample.Application.Features.Templates.Get;

using VsaSample.Application.Features.Templates.Shared;

internal sealed class GetTemplatesPagedQueryHandler(
    ITemplateRepository repository,
    ISieveProcessor sieve,
    ILogger<GetTemplatesPagedQueryHandler> logger)
    : IQueryHandler<GetTemplatesPagedQuery, PagedResult<TemplateResponse>>
{
    public async Task<Result<PagedResult<TemplateResponse>>> Handle(GetTemplatesPagedQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Template> source = repository.Query;

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim().ToLowerInvariant();
            source = source.Where(t =>
                t.Name.ToLower().Contains(term) ||
                (t.Description != null && t.Description.ToLower().Contains(term)));
        }

        var filtered = sieve.Apply(query.Sieve, source, applyPagination: false);
        var total = await filtered.CountAsync(cancellationToken);

        var paged = sieve.Apply(query.Sieve, filtered, applyPagination: true);
        var items = await paged
            .Select(t => t.ToResponse())
            .ToListAsync(cancellationToken);

        var pageSize = query.Sieve.PageSize ?? 10;
        var pageNumber = query.Sieve.Page ?? 1;
        var payload = PagedResult<TemplateResponse>.Create(items, pageNumber, pageSize, total);

        logger.LogInformation(
            "Fetched templates page {Page}/{Size} (Total: {Total})",
            pageNumber,
            pageSize,
            total);

        return Result<PagedResult<TemplateResponse>>.Success(payload);
    }
}
