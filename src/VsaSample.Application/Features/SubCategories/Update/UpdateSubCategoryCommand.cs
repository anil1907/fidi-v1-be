namespace VsaSample.Application.Features.SubCategories.Update;

public sealed record UpdateSubCategoryCommand(long Id, string Name) : ICommand;

public sealed record UpdateSubCategoryBody(string Name);

