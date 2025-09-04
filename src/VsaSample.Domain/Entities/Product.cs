namespace VsaSample.Domain.Entities;

public sealed class Product : BaseEntity, ITranslatable<ProductTranslation>
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Sku { get; set; } = null!;

    [Sieve(CanFilter = true, CanSort = true)]
    public decimal Price { get; set; }

    // Foreign key and reference navigation
    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    // Collection navigation
    public List<SubCategory> SubCategories { get; } = [];

    public List<ProductTranslation> Translations { get; } = [];

}
