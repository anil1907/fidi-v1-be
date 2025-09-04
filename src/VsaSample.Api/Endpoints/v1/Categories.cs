using VsaSample.Application.Features.Categories;
using VsaSample.Application.Features.Categories.Create;
using VsaSample.Application.Features.Categories.Delete;
using VsaSample.Application.Features.Categories.Get;
using VsaSample.Application.Features.Categories.GetById;
using VsaSample.Application.Features.Categories.Shared;
using VsaSample.Application.Features.Categories.Update;

namespace VsaSample.Api.Endpoints.v1;

public class Categories : EndpointGroupBase
{
    public override void Map(IEndpointRouteBuilder app)
    {
        var route = MapGroup(app)
            .MapToApiVersion(ApiEndpoints.V1);

        route.MapPost("",
                async (CreateCategoryCommand cmd, ICommandHandler<CreateCategoryCommand, long> handler,
                        CancellationToken ct) =>
                    (await handler.Handle(cmd, ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Categories.CreateCategory, ApiEndpoints.V1))
            .Produces<long>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .MapToApiVersion(ApiEndpoints.V1)
            .AcceptsJson<CreateCategoryCommand>()
            .HasPermission(Permissions.Policy(CategoriesFeature.Permissions.Instance.Create));

        route.MapDelete("/{id:long}",
                async (long id, ICommandHandler<DeleteCategoryCommand> handler, CancellationToken ct) =>
                    (await handler.Handle(new DeleteCategoryCommand(id), ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Categories.DeleteCategory, ApiEndpoints.V1))
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(CategoriesFeature.Permissions.Instance.Delete));

        route.MapGet("",
                async ([AsParameters] SieveModel sieve,
                        IQueryHandler<GetCategoriesPagedQuery, PagedResult<CategoryResponse>> handler,
                        CancellationToken ct) =>
                    (await handler.Handle(new GetCategoriesPagedQuery(sieve), ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Categories.GetCategoriesPage, ApiEndpoints.V1))
            .AcceptsRequest<GetCategoriesPagedQuery>()
            .Produces<PagedResult<CategoryResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(CategoriesFeature.Permissions.Instance.Read));

        route.MapGet("/{id:long}",
                async (long id, IQueryHandler<GetCategoryByIdQuery, CategoryResponse> handler, CancellationToken ct) =>
                    (await handler.Handle(new GetCategoryByIdQuery(id), ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Categories.GetCategoryById, ApiEndpoints.V1))
            .Produces<CategoryResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(CategoriesFeature.Permissions.Instance.Read));

        route.MapPut("{id:long}",
                async (long id, UpdateCategoryBody body, ICommandHandler<UpdateCategoryCommand> handler,
                        CancellationToken ct) =>
                    (await handler.Handle(new UpdateCategoryCommand(id, body.Name), ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Categories.UpdateCategory, ApiEndpoints.V1))
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<UpdateCategoryBody>()
            .HasPermission(Permissions.Policy(CategoriesFeature.Permissions.Instance.Update));
    }
}
