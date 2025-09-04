namespace VsaSample.Application.Features.SubCategories.Get;

public sealed record GetSubCategoriesPagedQuery(SieveModel Sieve)
    : IQuery<PagedResult<SubCategoryResponse>>;

