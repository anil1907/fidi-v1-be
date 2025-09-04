namespace VsaSample.Application.Features.Products.Get;

public sealed record GetProductsPagedQuery(
    SieveModel Sieve,
    string Culture,
    string DefaultCulture
) : IQuery<PagedResult<ProductResponse>>;