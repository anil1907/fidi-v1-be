namespace VsaSample.Application.Features.SubCategories.GetById;

internal sealed class GetSubCategoryByIdQueryHandler(
    IApplicationDbContext db,
    ICacheService cache,
    ILogger<GetSubCategoryByIdQueryHandler> logger)
    : IQueryHandler<GetSubCategoryByIdQuery, SubCategoryResponse>
{
    public async Task<Result<SubCategoryResponse>> Handle(GetSubCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var cacheKey = SubCategoriesFeature.Instance.TagTemplates.ById.Replace("{Id}", query.Id.ToString());

        var response = await cache.GetOrCreateAsync(
            cacheKey,
            async ct =>
            {
                var entity = await db.SubCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == query.Id, ct);
                return entity?.ToResponse();
            },
            cancellationToken);

        if (response is null)
        {
            return Result<SubCategoryResponse>.Failure(SubCategoryConstants.Errors.NotFoundById(query.Id));
        }

        logger.LogInformation("Fetched sub category {SubCategoryId}", query.Id);
        return Result<SubCategoryResponse>.Success(response);
    }
}

