namespace VsaSample.Domain.Entities;

public sealed class ProductTranslation : TranslationBaseEntity
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public Product ProductRef { get; set; } = null!;
}
