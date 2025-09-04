namespace VsaSample.Application.Features.Products.Delete;

public sealed record DeleteProductCommand(long Id) : ICommand;