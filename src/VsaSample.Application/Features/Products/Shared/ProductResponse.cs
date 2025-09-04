namespace VsaSample.Application.Features.Products.Shared;

public sealed record ProductResponse
{
    public long Id { get; init; }
    public string Sku { get; init; } = null!;
    public decimal Price { get; init; }
    public bool IsActive { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
}
