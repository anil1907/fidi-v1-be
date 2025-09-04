namespace VsaSample.Application.Features.Products.Shared;

public static class ProductMappings
{
    // Expression for EF projection (server-side)
    public static readonly Expression<Func<Product, ProductTranslation?, ProductTranslation?, ProductResponse>> ToResponseExpr =
        (e, tr, d) => new ProductResponse
        {
            Id = e.Id,
            Sku = e.Sku,
            Price = e.Price,
            IsActive = e.IsActive,
            Name = (tr ?? d)!.Name,
            Description = (tr ?? d)!.Description,
        };

    // Extension for in-memory mapping (fallback logic)
    public static ProductResponse ToResponse(this Product e, string culture, string defaultCulture)
    {
        var tr = e.Translations.FirstOrDefault(t => t.Culture == culture)
              ?? e.Translations.FirstOrDefault(t => t.Culture == defaultCulture);

        return new ProductResponse
        {
            Id = e.Id,
            Sku = e.Sku,
            Price = e.Price,
            IsActive = e.IsActive,
            Name = tr?.Name ?? string.Empty,
            Description = tr?.Description,
        };
    }
    public static IQueryable<ProductResponse> ProjectToResponse(
        this IQueryable<Product> source,
        Expression<Func<Product, ProductResponse>> selector)
        => source.Select(selector);

    public static IEnumerable<ProductResponse> MapToResponse(
        this IEnumerable<Product> source,
        Func<Product, ProductResponse> selector)
        => source.Select(selector);

    public static List<ProductResponse> ToResponseList(
        this IEnumerable<Product> source,
        Func<Product, ProductResponse> selector)
        => source.Select(selector).ToList();
}
