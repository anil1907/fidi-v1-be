namespace VsaSample.Application.Features.Categories.Update;

public sealed record UpdateCategoryCommand(Guid Id, string Name) : ICommand;

public sealed record UpdateCategoryBody(string Name);
