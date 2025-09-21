namespace VsaSample.Application.Features.SubCategories.Create;

internal sealed class CreateSubCategoryCommandHandler(
    IApplicationDbContext db,
    ICacheService cache,
    ILogger<CreateSubCategoryCommandHandler> logger)
    : ICommandHandler<CreateSubCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSubCategoryCommand command, CancellationToken cancellationToken)
    {
        var categoryExists = await db.Categories.AnyAsync(x => x.Id == command.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return Result<Guid>.Failure(CategoryConstants.Errors.NotFoundById(command.CategoryId));
        }

        var entity = new SubCategory { Name = command.Name, CategoryId = command.CategoryId };

        db.SubCategories.Add(entity);
        await db.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(SubCategoriesFeature.Instance.Cache.ListTag, cancellationToken);

        logger.LogInformation("SubCategory created with Id {Id}", entity.Id);
        return Result<Guid>.Success(entity.Id);
    }
}
