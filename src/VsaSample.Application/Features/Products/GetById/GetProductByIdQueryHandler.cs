namespace VsaSample.Application.Features.Products.GetById;

internal sealed class GetProductByIdQueryHandler(
    IProductRepository repository,
    ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(query.Id, cancellationToken);

        if (entity is null)
        {
            return Result<ProductResponse>.Failure(ProductConstants.Errors.NotFoundById(query.Id));
        }

        logger.LogInformation("Retrieved product {ProductId}", query.Id);

        var dto = entity.ToResponse(query.Culture, query.DefaultCulture);
        return Result<ProductResponse>.Success(dto);
    }
}
