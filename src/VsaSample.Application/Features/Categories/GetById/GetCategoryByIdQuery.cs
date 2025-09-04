namespace VsaSample.Application.Features.Categories.GetById;

public sealed record GetCategoryByIdQuery(long Id) : IQuery<CategoryResponse>;