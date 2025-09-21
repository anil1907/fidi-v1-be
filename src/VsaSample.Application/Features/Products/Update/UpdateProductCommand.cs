namespace VsaSample.Application.Features.Products.Update;

public sealed record UpdateProductCommand(Guid Id, string Culture, string Name, string? Description)
    : ICommand;

public sealed record UpdateProduct(string Name, string? Description);
