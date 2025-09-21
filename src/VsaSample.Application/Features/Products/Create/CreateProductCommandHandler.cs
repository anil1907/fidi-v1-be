namespace VsaSample.Application.Features.Products.Create;

public sealed class CreateProductCommandHandler(IProductRepository repository, ILogger<CreateProductCommandHandler> logger)
    : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var entity = new Product
        {
            Sku = command.Sku,
            Price = command.Price,
            IsActive = command.IsActive
        };

        foreach (var tr in command.Translations)
        {
            entity.Translations.Add(new ProductTranslation
            {
                Culture = tr.Culture,
                Name = tr.Name,
                Description = tr.Description
            });
        }

        var id = await repository.AddAsync(entity, cancellationToken);

        logger.LogInformation("Product created with Id {Id} (SKU: {Sku})", id, entity.Sku);
        return Result<Guid>.Success(id);
    }
}
