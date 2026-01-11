namespace VsaSample.Domain.Entities;

public sealed class Organization : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; } = string.Empty;
}
