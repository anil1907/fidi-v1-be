namespace VsaSample.Application.Features.SubCategories.Shared;

public sealed record SubCategoryResponse(Guid Id, Guid CategoryId, string Name, bool IsActive);
