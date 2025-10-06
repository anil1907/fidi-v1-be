using VsaSample.Application.Features.DietPlans;
using VsaSample.Application.Features.DietPlans.Create;
using VsaSample.Application.Features.DietPlans.Delete;
using VsaSample.Application.Features.DietPlans.Get;
using VsaSample.Application.Features.DietPlans.GetById;
using VsaSample.Application.Features.DietPlans.Shared;
using VsaSample.Application.Features.DietPlans.Update;

namespace VsaSample.Api.Endpoints.v1;

internal sealed class DietPlans : EndpointGroupBase
{
    public override void Map(IEndpointRouteBuilder app)
    {
        var route = MapGroup(app)
            .MapToApiVersion(ApiEndpoints.V1);

        route.MapGet("",
                async ([AsParameters] SieveModel sieve,
                        string? search,
                        Guid? clientId,
                        Guid? templateId,
                        DateOnly? startsOnOrAfter,
                        DateOnly? endsOnOrBefore,
                        bool? isActive,
                        IQueryHandler<GetDietPlansPagedQuery, PagedResult<DietPlanResponse>> handler,
                        CancellationToken ct) =>
                {
                    var query = new GetDietPlansPagedQuery(sieve, search, clientId, templateId, startsOnOrAfter, endsOnOrBefore, isActive);
                    var result = await handler.Handle(query, ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.DietPlans.GetDietPlansPaged, ApiEndpoints.V1))
            .Produces<PagedResult<DietPlanResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(DietPlansFeature.Permissions.Instance.Read));

        route.MapGet("/{id:guid}",
                async (Guid id, IQueryHandler<GetDietPlanByIdQuery, DietPlanResponse> handler, CancellationToken ct) =>
                {
                    var result = await handler.Handle(new GetDietPlanByIdQuery(id), ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.DietPlans.GetDietPlanById, ApiEndpoints.V1))
            .Produces<DietPlanResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(DietPlansFeature.Permissions.Instance.Read));

        route.MapPost("",
                async (CreateDietPlanCommand command, ICommandHandler<CreateDietPlanCommand, Guid> handler,
                        CancellationToken ct) =>
                {
                    var result = await handler.Handle(command, ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.Created($"/api/v1/dietplans/{result.Value}", new { Id = result.Value });
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.DietPlans.CreateDietPlan, ApiEndpoints.V1))
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<CreateDietPlanCommand>()
            .HasPermission(Permissions.Policy(DietPlansFeature.Permissions.Instance.Create));

        route.MapPut("/{id:guid}",
                async (Guid id, UpdateDietPlanRequest request,
                        ICommandHandler<UpdateDietPlanCommand> handler,
                        CancellationToken ct) =>
                {
                    var command = new UpdateDietPlanCommand(
                        id,
                        request.ClientId,
                        request.TemplateId,
                        request.Name,
                        request.DateStart,
                        request.DateEnd,
                        request.Notes,
                        request.Sections,
                        request.IsActive);

                    var result = await handler.Handle(command, ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.NoContent();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.DietPlans.UpdateDietPlan, ApiEndpoints.V1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<UpdateDietPlanRequest>()
            .HasPermission(Permissions.Policy(DietPlansFeature.Permissions.Instance.Update));

        route.MapDelete("/{id:guid}",
                async (Guid id, ICommandHandler<DeleteDietPlanCommand> handler, CancellationToken ct) =>
                {
                    var result = await handler.Handle(new DeleteDietPlanCommand(id), ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.NoContent();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.DietPlans.DeleteDietPlan, ApiEndpoints.V1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(DietPlansFeature.Permissions.Instance.Delete));
    }
}
