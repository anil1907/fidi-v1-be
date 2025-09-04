namespace VsaSample.Application.Features.Products.GetById;

public sealed record GetProductByIdQuery(long Id, string Culture, string DefaultCulture)
    : IQuery<ProductResponse>;