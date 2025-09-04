namespace VsaSample.Application.Features.SubCategories.Delete;

internal sealed class DeleteSubCategoryCommandHandler(IApplicationDbContext db, ICacheService cache, ILogger<DeleteSubCategoryCommandHandler> logger)
    : ICommandHandler<DeleteSubCategoryCommand>
{
    public async Task<Result> Handle(DeleteSubCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = await db.SubCategories.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
        if (entity is null)
        {
            return Result.Failure(SubCategoryConstants.Errors.NotFoundById(command.Id));
        }

        db.SubCategories.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);

        var cacheKey = SubCategoriesFeature.Instance.TagTemplates.ById.Replace("{Id}", command.Id.ToString());
        await cache.RemoveAsync(cacheKey, cancellationToken);
        await cache.RemoveAsync(SubCategoriesFeature.Instance.Cache.ListTag, cancellationToken);

        logger.LogInformation("SubCategory {SubCategoryId} deleted.", command.Id);
        return Result.Success();
    }
}

