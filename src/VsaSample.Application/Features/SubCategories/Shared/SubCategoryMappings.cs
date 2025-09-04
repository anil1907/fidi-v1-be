namespace VsaSample.Application.Features.SubCategories.Shared;

public static class SubCategoryMappings
{
    public static readonly Expression<Func<SubCategory, SubCategoryResponse>> ToResponseExpr =
        e => new SubCategoryResponse(e.Id, e.CategoryId, e.Name, e.IsActive);

    public static SubCategoryResponse ToResponse(this SubCategory e) => new SubCategoryResponse(e.Id, e.CategoryId, e.Name, e.IsActive);
}

