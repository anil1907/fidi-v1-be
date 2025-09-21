using VsaSample.Application.Features.Products;
using VsaSample.Application.Features.Products.Create;
using VsaSample.Application.Features.Products.Delete;
using VsaSample.Application.Features.Products.Get;
using VsaSample.Application.Features.Products.GetById;
using VsaSample.Application.Features.Products.Shared;
using VsaSample.Application.Features.Products.Update;

namespace VsaSample.Api.Endpoints.v1;

public class Products : EndpointGroupBase
{
    public override void Map(IEndpointRouteBuilder app)
    {
        var route = MapGroup(app)
            .MapToApiVersion(ApiEndpoints.V1);

        route.MapPost("",
                async (CreateProductCommand cmd, ICommandHandler<CreateProductCommand, Guid> handler,
                        CancellationToken ct) =>
                    (await handler.Handle(cmd, ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Products.CreateProduct, ApiEndpoints.V1))
            .Produces<Guid>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<CreateProductCommand>()
            .HasPermission(Permissions.Policy(ProductsFeature.Permissions.Instance.Create));

        route.MapDelete("/{id:guid}",
                async (Guid id, ICommandHandler<DeleteProductCommand> handler, CancellationToken ct) =>
                    (await handler.Handle(new DeleteProductCommand(id), ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Products.DeleteProduct, ApiEndpoints.V1))
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(ProductsFeature.Permissions.Instance.Delete));

        route.MapGet("",
                async ([AsParameters] SieveModel sieve, string? culture,
                    IQueryHandler<GetProductsPagedQuery, PagedResult<ProductResponse>> handler, CancellationToken ct) =>
                {
                    var reqCulture = culture ?? Cultures.Default;
                    var result = await handler.Handle(new GetProductsPagedQuery(sieve, reqCulture, Cultures.Default),
                        ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Products.GetProductsPaged, ApiEndpoints.V1))
            .Produces<PagedResult<ProductResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(ProductsFeature.Permissions.Instance.Read));

        route.MapGet("/{id:guid}",
                async (Guid id, string? culture, IQueryHandler<GetProductByIdQuery, ProductResponse> handler,
                    CancellationToken ct) =>
                {
                    var reqCulture = culture ?? Cultures.Default;
                    var result = await handler.Handle(new GetProductByIdQuery(id, reqCulture, Cultures.Default), ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Products.GetProductById, ApiEndpoints.V1))
            .AcceptsRequest<GetProductByIdQuery>()
            .Produces<ProductResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(ProductsFeature.Permissions.Instance.Read));

        route.MapPut("/{id:guid}/translations/{culture}", async (Guid id, string culture, UpdateProduct body,
                ICommandHandler<UpdateProductCommand> handler, CancellationToken ct) =>
            {
                var cmd = new UpdateProductCommand(id, culture, body.Name, body.Description);
                var result = await handler.Handle(cmd, ct);
                return result.ToHttpResponse();
            })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Products.UpdateProduct, ApiEndpoints.V1))
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<UpdateProduct>()
            .HasPermission(Permissions.Policy(ProductsFeature.Permissions.Instance.Update));
    }
}
