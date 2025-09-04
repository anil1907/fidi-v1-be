namespace VsaSample.Application.Features.Categories.Shared;

public static class CategoryMappings
{
    public static readonly Expression<Func<Category, CategoryResponse>> ToResponseExpr =
        e => new CategoryResponse (e.Id, e.Name, e.IsActive);

    public static CategoryResponse ToResponse(this Category e) => new CategoryResponse (e.Id, e.Name, e.IsActive);
}
