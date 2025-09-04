namespace VsaSample.Application.Features.SubCategories.GetById;

public sealed record GetSubCategoryByIdQuery(long Id) : IQuery<SubCategoryResponse>;

