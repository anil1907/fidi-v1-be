namespace VsaSample.Application.Features.SubCategories.Shared;

public sealed record SubCategoryResponse(long Id, long CategoryId, string Name, bool IsActive);

