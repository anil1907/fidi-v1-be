namespace VsaSample.Application.Features.SubCategories.Create;

public sealed record CreateSubCategoryCommand(long CategoryId, string Name) : ICommand<long>;

