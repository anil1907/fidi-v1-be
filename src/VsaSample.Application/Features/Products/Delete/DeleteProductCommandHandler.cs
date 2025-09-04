namespace VsaSample.Application.Features.Products.Delete;

internal sealed class DeleteProductCommandHandler(IProductRepository repository, ILogger<DeleteProductCommandHandler> logger)
    : ICommandHandler<DeleteProductCommand>
{
    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (entity is null)
        {
            return Result.Failure(ProductConstants.Errors.NotFoundById(command.Id));
        }

        await repository.DeleteAsync(entity, cancellationToken);

        logger.LogInformation("Product {ProductId} deleted.", command.Id);
        return Result.Success();
    }
}
