namespace VsaSample.Application.Features.Products.GetById;

public sealed record GetProductByIdQuery(Guid Id, string Culture, string DefaultCulture)
    : IQuery<ProductResponse>;
