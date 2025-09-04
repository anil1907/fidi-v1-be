namespace VsaSample.Application.Features.Categories.Update;

internal sealed class UpdateCategoryCommandHandler(ICategoryRepository repository, ILogger<UpdateCategoryCommandHandler> logger)
    : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (entity is null)
        {
            return Result.Failure(CategoryConstants.Errors.NotFoundById(command.Id));
        }

        entity.Name = command.Name;
        await repository.UpdateAsync(entity, cancellationToken);

        logger.LogInformation("Category {CategoryId} updated.", command.Id);
        return Result.Success();
    }
}
