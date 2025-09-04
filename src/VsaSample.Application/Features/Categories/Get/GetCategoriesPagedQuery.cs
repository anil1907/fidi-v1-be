namespace VsaSample.Application.Features.Categories.Get;

public sealed record GetCategoriesPagedQuery(SieveModel Sieve)
    : IQuery<PagedResult<CategoryResponse>>;