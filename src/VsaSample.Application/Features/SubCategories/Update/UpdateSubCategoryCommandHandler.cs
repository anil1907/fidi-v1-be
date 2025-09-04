namespace VsaSample.Application.Features.SubCategories.Update;

internal sealed class UpdateSubCategoryCommandHandler(IApplicationDbContext db, ICacheService cache, ILogger<UpdateSubCategoryCommandHandler> logger)
    : ICommandHandler<UpdateSubCategoryCommand>
{
    public async Task<Result> Handle(UpdateSubCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = await db.SubCategories.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
        if (entity is null)
        {
            return Result.Failure(SubCategoryConstants.Errors.NotFoundById(command.Id));
        }

        entity.Name = command.Name;
        await db.SaveChangesAsync(cancellationToken);

        var response = entity.ToResponse();
        var cacheKey = SubCategoriesFeature.Instance.TagTemplates.ById.Replace("{Id}", entity.Id.ToString());
        await cache.SetAsync(cacheKey, response, cancellationToken);
        await cache.RemoveAsync(SubCategoriesFeature.Instance.Cache.ListTag, cancellationToken);

        logger.LogInformation("SubCategory {SubCategoryId} updated.", command.Id);
        return Result.Success();
    }
}

