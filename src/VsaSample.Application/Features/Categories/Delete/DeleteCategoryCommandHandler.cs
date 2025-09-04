namespace VsaSample.Application.Features.Categories.Delete;

internal sealed class DeleteCategoryCommandHandler(ICategoryRepository repository, ILogger<DeleteCategoryCommandHandler> logger)
    : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (entity is null)
        {
            return Result.Failure(CategoryConstants.Errors.NotFoundById(command.Id));
        }

        await repository.DeleteAsync(entity, cancellationToken);

        logger.LogInformation("Category {CategoryId} deleted.", command.Id);
        return Result.Success();
    }
}
