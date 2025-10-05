using VsaSample.Application.Features.Templates;
using VsaSample.Application.Features.Templates.Create;
using VsaSample.Application.Features.Templates.Delete;
using VsaSample.Application.Features.Templates.Get;
using VsaSample.Application.Features.Templates.GetById;
using VsaSample.Application.Features.Templates.Shared;
using VsaSample.Application.Features.Templates.Update;

namespace VsaSample.Api.Endpoints.v1;

internal sealed class Templates : EndpointGroupBase
{
    public override void Map(IEndpointRouteBuilder app)
    {
        var route = MapGroup(app)
            .MapToApiVersion(ApiEndpoints.V1);

        route.MapGet("",
                async ([AsParameters] SieveModel sieve,
                        string? search,
                        IQueryHandler<GetTemplatesPagedQuery, PagedResult<TemplateResponse>> handler,
                        CancellationToken ct) =>
                {
                    var query = new GetTemplatesPagedQuery(sieve, search);
                    var result = await handler.Handle(query, ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Templates.GetTemplatesPaged, ApiEndpoints.V1))
            .Produces<PagedResult<TemplateResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(TemplatesFeature.Permissions.Instance.Read));

        route.MapGet("/{id:guid}",
                async (Guid id, IQueryHandler<GetTemplateByIdQuery, TemplateResponse> handler, CancellationToken ct) =>
                {
                    var result = await handler.Handle(new GetTemplateByIdQuery(id), ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Templates.GetTemplateById, ApiEndpoints.V1))
            .Produces<TemplateResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(TemplatesFeature.Permissions.Instance.Read));

        route.MapPost("",
                async (CreateTemplateCommand command, ICommandHandler<CreateTemplateCommand, Guid> handler,
                        CancellationToken ct) =>
                {
                    var result = await handler.Handle(command, ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.Created($"/api/v1/templates/{result.Value}", new { Id = result.Value });
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Templates.CreateTemplate, ApiEndpoints.V1))
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<CreateTemplateCommand>()
            .HasPermission(Permissions.Policy(TemplatesFeature.Permissions.Instance.Create));

        route.MapPut("/{id:guid}",
                async (Guid id, UpdateTemplateRequest request,
                        ICommandHandler<UpdateTemplateCommand> handler,
                        CancellationToken ct) =>
                {
                    var command = new UpdateTemplateCommand(
                        id,
                        request.Name,
                        request.Description,
                        request.Sections,
                        request.IsActive);

                    var result = await handler.Handle(command, ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.NoContent();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Templates.UpdateTemplate, ApiEndpoints.V1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<UpdateTemplateRequest>()
            .HasPermission(Permissions.Policy(TemplatesFeature.Permissions.Instance.Update));

        route.MapDelete("/{id:guid}",
                async (Guid id, ICommandHandler<DeleteTemplateCommand> handler, CancellationToken ct) =>
                {
                    var result = await handler.Handle(new DeleteTemplateCommand(id), ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.NoContent();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Templates.DeleteTemplate, ApiEndpoints.V1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(TemplatesFeature.Permissions.Instance.Delete));
    }
}
