namespace VsaSample.Application.Features.SubCategories.Create;

internal sealed class CreateSubCategoryCommandHandler(IApplicationDbContext db, ICacheService cache, ILogger<CreateSubCategoryCommandHandler> logger)
    : ICommandHandler<CreateSubCategoryCommand, long>
{
    public async Task<Result<long>> Handle(CreateSubCategoryCommand command, CancellationToken cancellationToken)
    {
        var categoryExists = await db.Categories.AnyAsync(x => x.Id == command.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return Result<long>.Failure(CategoryConstants.Errors.NotFoundById(command.CategoryId));
        }

        var entity = new SubCategory { Name = command.Name, CategoryId = command.CategoryId };
        db.SubCategories.Add(entity);
        await db.SaveChangesAsync(cancellationToken);

        var response = entity.ToResponse();
        var cacheKey = SubCategoriesFeature.Instance.TagTemplates.ById.Replace("{Id}", entity.Id.ToString());
        await cache.SetAsync(cacheKey, response, cancellationToken);
        await cache.RemoveAsync(SubCategoriesFeature.Instance.Cache.ListTag, cancellationToken);

        logger.LogInformation("SubCategory created with Id {Id}", entity.Id);
        return Result<long>.Success(entity.Id);
    }
}

