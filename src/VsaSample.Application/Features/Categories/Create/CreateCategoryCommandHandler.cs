namespace VsaSample.Application.Features.Categories.Create;

public sealed class CreateCategoryCommandHandler(ICategoryRepository repository, ILogger<CreateCategoryCommandHandler> logger)
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = new Category { Name = command.Name };
        var id = await repository.AddAsync(entity, cancellationToken);

        logger.LogInformation("Category created with Id {Id}", id);
        return Result<Guid>.Success(id);
    }
}
