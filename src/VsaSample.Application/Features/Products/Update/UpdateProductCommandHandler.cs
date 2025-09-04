namespace VsaSample.Application.Features.Products.Update;

internal sealed class UpdateProductCommandHandler(IApplicationDbContext db, ILogger<UpdateProductCommandHandler> logger)
    : ICommandHandler<UpdateProductCommand>
{
    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var translation = await db.ProductTranslation
            .FirstOrDefaultAsync(t => t.ProductId == command.Id && t.Culture == command.Culture, cancellationToken);

        if (translation is null)
        {
            translation = new ProductTranslation
            {
                ProductId = command.Id,
                Culture = command.Culture,
                Name = command.Name,
                Description = command.Description
            };
            db.ProductTranslation.Add(translation);
        }
        else
        {
            translation.Name = command.Name;
            translation.Description = command.Description;
        }

        await db.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Product {ProductId} translation upserted for culture {Culture}.",
            command.Id,
            command.Culture);

        return Result.Success();
    }
}
