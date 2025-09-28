using VsaSample.Application.Features.Clients;
using VsaSample.Application.Features.Clients.Create;
using VsaSample.Application.Features.Clients.Delete;
using VsaSample.Application.Features.Clients.Get;
using VsaSample.Application.Features.Clients.GetById;
using VsaSample.Application.Features.Clients.Shared;
using VsaSample.Application.Features.Clients.Update;

namespace VsaSample.Api.Endpoints.v1;

internal sealed class Clients : EndpointGroupBase
{
    public override void Map(IEndpointRouteBuilder app)
    {
        var route = MapGroup(app)
            .MapToApiVersion(ApiEndpoints.V1);

        route.MapGet("",
                async ([AsParameters] SieveModel sieve,
                        string? search,
                        IQueryHandler<GetClientsPagedQuery, PagedResult<ClientResponse>> handler,
                        CancellationToken ct) =>
                {
                    var query = new GetClientsPagedQuery(sieve, search);
                    var result = await handler.Handle(query, ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Clients.GetClientsPaged, ApiEndpoints.V1))
            .Produces<PagedResult<ClientResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(ClientsFeature.Permissions.Instance.Read));

        route.MapGet("/{id:guid}",
                async (Guid id, IQueryHandler<GetClientByIdQuery, ClientResponse> handler, CancellationToken ct) =>
                {
                    var result = await handler.Handle(new GetClientByIdQuery(id), ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Clients.GetClientById, ApiEndpoints.V1))
            .Produces<ClientResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(ClientsFeature.Permissions.Instance.Read));

        route.MapPost("",
                async (CreateClientCommand command, ICommandHandler<CreateClientCommand, Guid> handler,
                        CancellationToken ct) =>
                {
                    var result = await handler.Handle(command, ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.Created($"/api/v1/clients/{result.Value}", new { Id = result.Value });
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Clients.CreateClient, ApiEndpoints.V1))
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<CreateClientCommand>()
            .HasPermission(Permissions.Policy(ClientsFeature.Permissions.Instance.Create));

        route.MapPut("/{id:guid}",
                async (Guid id, UpdateClientRequest request,
                        ICommandHandler<UpdateClientCommand> handler,
                        CancellationToken ct) =>
                {
                    var command = new UpdateClientCommand(
                        id,
                        request.FirstName,
                        request.LastName,
                        request.Email,
                        request.Phone,
                        request.Notes,
                        request.Goals,
                        request.IsActive);

                    var result = await handler.Handle(command, ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.NoContent();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Clients.UpdateClient, ApiEndpoints.V1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<UpdateClientRequest>()
            .HasPermission(Permissions.Policy(ClientsFeature.Permissions.Instance.Update));

        route.MapDelete("/{id:guid}",
                async (Guid id, ICommandHandler<DeleteClientCommand> handler, CancellationToken ct) =>
                {
                    var result = await handler.Handle(new DeleteClientCommand(id), ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.NoContent();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Clients.DeleteClient, ApiEndpoints.V1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(ClientsFeature.Permissions.Instance.Delete));
    }
}
