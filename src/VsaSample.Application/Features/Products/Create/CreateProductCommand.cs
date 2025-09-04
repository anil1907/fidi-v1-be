namespace VsaSample.Application.Features.Products.Create;

public sealed record CreateProductCommand(
    string Sku,
    decimal Price,
    bool IsActive,
    string DefaultCulture,
    List<CreateProductTranslation> Translations
) : ICommand<long>;

public sealed record CreateProductTranslation(string Culture, string Name, string? Description);
