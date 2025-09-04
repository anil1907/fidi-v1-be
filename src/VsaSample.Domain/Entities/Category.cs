namespace VsaSample.Domain.Entities;

public sealed class Category : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; } = null!;

    public List<Product> Products { get; } = [];

    public List<SubCategory> SubCategories { get; } = [];

}
