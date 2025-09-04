namespace VsaSample.Domain.Entities;

public sealed class SubCategory : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; } = null!;

    [Sieve(CanFilter = true, CanSort = true)]
    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    [Sieve(CanFilter = true, CanSort = true)]
    public long ProductId { get; set; }
    public Product Product { get; set; } = null!;
}

