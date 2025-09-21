namespace VsaSample.Application.Features.SubCategories.GetById;

public sealed record GetSubCategoryByIdQuery(Guid Id) : IQuery<SubCategoryResponse>;
