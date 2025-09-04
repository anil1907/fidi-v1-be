namespace VsaSample.Application.Features.Categories.Create;

public sealed class CreateCategoryCommandHandler(ICategoryRepository repository, ILogger<CreateCategoryCommandHandler> logger)
    : ICommandHandler<CreateCategoryCommand, long>
{
    public async Task<Result<long>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = new Category { Name = command.Name };
        var id = await repository.AddAsync(entity, cancellationToken);

        logger.LogInformation("Category created with Id {Id}", id);
        return Result<long>.Success(id);
    }
}
