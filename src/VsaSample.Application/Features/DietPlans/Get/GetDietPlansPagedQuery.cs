using VsaSample.Application.Features.DietPlans.Shared;

namespace VsaSample.Application.Features.DietPlans.Get;

public sealed record GetDietPlansPagedQuery(
    SieveModel Sieve,
    string? Search,
    Guid? ClientId,
    Guid? TemplateId,
    DateOnly? StartsOnOrAfter,
    DateOnly? EndsOnOrBefore,
    bool? IsActive)
    : IQuery<PagedResult<DietPlanResponse>>;
