namespace VsaSample.Application.Features.Products.Delete;

public sealed record DeleteProductCommand(Guid Id) : ICommand;
