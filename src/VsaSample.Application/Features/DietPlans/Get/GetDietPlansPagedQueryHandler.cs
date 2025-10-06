using VsaSample.Application.Features.DietPlans.Shared;

namespace VsaSample.Application.Features.DietPlans.Get;

internal sealed class GetDietPlansPagedQueryHandler(
    IDietPlanRepository repository,
    ISieveProcessor sieve,
    ILogger<GetDietPlansPagedQueryHandler> logger)
    : IQueryHandler<GetDietPlansPagedQuery, PagedResult<DietPlanResponse>>
{
    public async Task<Result<PagedResult<DietPlanResponse>>> Handle(GetDietPlansPagedQuery query, CancellationToken cancellationToken)
    {
        IQueryable<DietPlan> plans = repository.Query;

        if (query.ClientId is Guid clientId)
        {
            plans = plans.Where(plan => plan.ClientId == clientId);
        }

        if (query.TemplateId is Guid templateId)
        {
            plans = plans.Where(plan => plan.TemplateId == templateId);
        }

        if (query.IsActive.HasValue)
        {
            plans = plans.Where(plan => plan.IsActive == query.IsActive.Value);
        }

        if (query.StartsOnOrAfter.HasValue)
        {
            var start = query.StartsOnOrAfter.Value;
            plans = plans.Where(plan => plan.DateStart >= start);
        }

        if (query.EndsOnOrBefore.HasValue)
        {
            var end = query.EndsOnOrBefore.Value;
            plans = plans.Where(plan => plan.DateEnd <= end);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim().ToLowerInvariant();
            plans = plans.Where(plan =>
                plan.Name.ToLower().Contains(term) ||
                (plan.Notes != null && plan.Notes.ToLower().Contains(term)));
        }

        var filtered = sieve.Apply(query.Sieve, plans, applyPagination: false);
        var total = await filtered.CountAsync(cancellationToken);

        var paged = sieve.Apply(query.Sieve, filtered, applyPagination: true);
        var items = await paged
            .Select(plan => plan.ToResponse())
            .ToListAsync(cancellationToken);

        var pageSize = query.Sieve.PageSize ?? 10;
        var pageNumber = query.Sieve.Page ?? 1;
        var payload = PagedResult<DietPlanResponse>.Create(items, pageNumber, pageSize, total);

        logger.LogInformation(
            "Fetched diet plans page {Page}/{Size} (Total: {Total})",
            pageNumber,
            pageSize,
            total);

        return Result<PagedResult<DietPlanResponse>>.Success(payload);
    }
}

