namespace VsaSample.Application.Features.Categories.GetById;

internal sealed class GetCategoryByIdQueryHandler(
    ICategoryRepository repository,
    ILogger<GetCategoryByIdQueryHandler> logger)
    : IQueryHandler<GetCategoryByIdQuery, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(query.Id, cancellationToken);

        if (entity is null)
        {
            return Result<CategoryResponse>.Failure(CategoryConstants.Errors.NotFoundById(query.Id));
        }

        logger.LogInformation("Fetched category {CategoryId}", query.Id);
        return Result<CategoryResponse>.Success(entity.ToResponse());
    }
}
