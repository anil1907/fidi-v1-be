namespace VsaSample.Application.Features.Categories.Create;

public sealed record CreateCategoryCommand(string Name) : ICommand<long>;
