using VsaSample.Application.Features.SubCategories;
using VsaSample.Application.Features.SubCategories.Create;
using VsaSample.Application.Features.SubCategories.Delete;
using VsaSample.Application.Features.SubCategories.Export;
using VsaSample.Application.Features.SubCategories.Get;
using VsaSample.Application.Features.SubCategories.GetById;
using VsaSample.Application.Features.SubCategories.Import;
using VsaSample.Application.Features.SubCategories.Shared;
using VsaSample.Application.Features.SubCategories.Update;

namespace VsaSample.Api.Endpoints.v1;

public class SubCategories : EndpointGroupBase
{
    public override void Map(IEndpointRouteBuilder app)
    {
        var route = MapGroup(app)
            .MapToApiVersion(ApiEndpoints.V1);

        route.MapPost("",
                async (CreateSubCategoryCommand cmd, ICommandHandler<CreateSubCategoryCommand, Guid> handler,
                        CancellationToken ct) =>
                    (await handler.Handle(cmd, ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.SubCategories.CreateSubCategory, ApiEndpoints.V1))
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<CreateSubCategoryCommand>()
            .HasPermission(Permissions.Policy(SubCategoriesFeature.Permissions.Instance.Create));

        route.MapDelete("/{id:guid}",
                async (Guid id, ICommandHandler<DeleteSubCategoryCommand> handler, CancellationToken ct) =>
                    (await handler.Handle(new DeleteSubCategoryCommand(id), ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.SubCategories.DeleteSubCategory, ApiEndpoints.V1))
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(SubCategoriesFeature.Permissions.Instance.Delete));

        route.MapGet("",
                async ([AsParameters] SieveModel sieve,
                        IQueryHandler<GetSubCategoriesPagedQuery, PagedResult<SubCategoryResponse>> handler,
                        CancellationToken ct) =>
                    (await handler.Handle(new GetSubCategoriesPagedQuery(sieve), ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.SubCategories.GetSubCategoriesPage, ApiEndpoints.V1))
            .AcceptsRequest<GetSubCategoriesPagedQuery>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(SubCategoriesFeature.Permissions.Instance.Read));

        route.MapGet("/{id:guid}",
                async (Guid id, IQueryHandler<GetSubCategoryByIdQuery, SubCategoryResponse> handler,
                        CancellationToken ct) =>
                    (await handler.Handle(new GetSubCategoryByIdQuery(id), ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.SubCategories.GetSubCategoryById, ApiEndpoints.V1))
            .AcceptsRequest<GetSubCategoryByIdQuery>()
            .Produces<SubCategoryResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(SubCategoriesFeature.Permissions.Instance.Read));

        route.MapPut("{id:guid}",
                async (Guid id, UpdateSubCategoryBody body, ICommandHandler<UpdateSubCategoryCommand> handler,
                        CancellationToken ct) =>
                    (await handler.Handle(new UpdateSubCategoryCommand(id, body.Name), ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.SubCategories.UpdateSubCategory, ApiEndpoints.V1))
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<UpdateSubCategoryBody>()
            .HasPermission(Permissions.Policy(SubCategoriesFeature.Permissions.Instance.Update));

        route.MapGet("export", async (IQueryHandler<ExportSubCategoriesQuery, byte[]> handler, CancellationToken ct) =>
            {
                var result = await handler.Handle(new ExportSubCategoriesQuery(), ct);
                return result.IsSuccess
                    ? Results.File(result.Value, ContentTypes.ApplicationOfficeDocument, "subcategories.xlsx")
                    : result.ToHttpResponse();
            })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.SubCategories.ExportSubCategories, ApiEndpoints.V1))
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(SubCategoriesFeature.Permissions.Instance.Export));

        route.MapPost("import",
                async (IFormFile file, ICommandHandler<ImportSubCategoriesCommand, int> handler,
                    CancellationToken ct) =>
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms, ct);
                    var result = await handler.Handle(new ImportSubCategoriesCommand(ms.ToArray()), ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.SubCategories.ImportSubCategories, ApiEndpoints.V1))
            .Accepts<IFormFile>(ContentTypes.MultipartFormData)
            .Produces<int>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(SubCategoriesFeature.Permissions.Instance.Import));
    }
}
