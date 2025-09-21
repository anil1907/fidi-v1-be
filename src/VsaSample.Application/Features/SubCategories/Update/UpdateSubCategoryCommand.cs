namespace VsaSample.Application.Features.SubCategories.Update;

public sealed record UpdateSubCategoryCommand(Guid Id, string Name) : ICommand;

public sealed record UpdateSubCategoryBody(string Name);

