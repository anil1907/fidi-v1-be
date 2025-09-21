namespace VsaSample.Application.Features.Categories.GetById;

public sealed record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryResponse>;
