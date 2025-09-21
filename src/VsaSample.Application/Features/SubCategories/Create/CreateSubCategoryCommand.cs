namespace VsaSample.Application.Features.SubCategories.Create;

public sealed record CreateSubCategoryCommand(Guid CategoryId, string Name) : ICommand<Guid>;

