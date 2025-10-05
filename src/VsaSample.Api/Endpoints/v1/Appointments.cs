using VsaSample.Application.Features.Appointments;
using VsaSample.Domain.Enums;
using VsaSample.Application.Features.Appointments.Create;
using VsaSample.Application.Features.Appointments.Delete;
using VsaSample.Application.Features.Appointments.Get;
using VsaSample.Application.Features.Appointments.GetById;
using VsaSample.Application.Features.Appointments.Shared;
using VsaSample.Application.Features.Appointments.Update;

namespace VsaSample.Api.Endpoints.v1;

internal sealed class Appointments : EndpointGroupBase
{
    public override void Map(IEndpointRouteBuilder app)
    {
        var route = MapGroup(app)
            .MapToApiVersion(ApiEndpoints.V1);

        route.MapGet("",
                async ([AsParameters] SieveModel sieve,
                        string? search,
                        Guid? clientId,
                        AppointmentStatus? status,
                        DateTime? startsAfter,
                        DateTime? endsBefore,
                        IQueryHandler<GetAppointmentsPagedQuery, PagedResult<AppointmentResponse>> handler,
                        CancellationToken ct) =>
                {
                    var query = new GetAppointmentsPagedQuery(sieve, search, clientId, status, startsAfter, endsBefore);
                    var result = await handler.Handle(query, ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Appointments.GetAppointmentsPaged, ApiEndpoints.V1))
            .Produces<PagedResult<AppointmentResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(AppointmentsFeature.Permissions.Instance.Read));

        route.MapGet("/{id:guid}",
                async (Guid id, IQueryHandler<GetAppointmentByIdQuery, AppointmentResponse> handler, CancellationToken ct) =>
                {
                    var result = await handler.Handle(new GetAppointmentByIdQuery(id), ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Appointments.GetAppointmentById, ApiEndpoints.V1))
            .Produces<AppointmentResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(AppointmentsFeature.Permissions.Instance.Read));

        route.MapPost("",
                async (CreateAppointmentCommand command, ICommandHandler<CreateAppointmentCommand, Guid> handler,
                        CancellationToken ct) =>
                {
                    var result = await handler.Handle(command, ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.Created($"/api/v1/appointments/{result.Value}", new { Id = result.Value });
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Appointments.CreateAppointment, ApiEndpoints.V1))
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<CreateAppointmentCommand>()
            .HasPermission(Permissions.Policy(AppointmentsFeature.Permissions.Instance.Create));

        route.MapPut("/{id:guid}",
                async (Guid id, UpdateAppointmentRequest request,
                        ICommandHandler<UpdateAppointmentCommand> handler,
                        CancellationToken ct) =>
                {
                    var command = new UpdateAppointmentCommand(
                        id,
                        request.ClientId,
                        request.Title,
                        request.Description,
                        request.StartsAt,
                        request.EndsAt,
                        request.Status,
                        request.IsActive);

                    var result = await handler.Handle(command, ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.NoContent();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Appointments.UpdateAppointment, ApiEndpoints.V1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .AcceptsJson<UpdateAppointmentRequest>()
            .HasPermission(Permissions.Policy(AppointmentsFeature.Permissions.Instance.Update));

        route.MapDelete("/{id:guid}",
                async (Guid id, ICommandHandler<DeleteAppointmentCommand> handler, CancellationToken ct) =>
                {
                    var result = await handler.Handle(new DeleteAppointmentCommand(id), ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToHttpResponse();
                    }

                    return Results.NoContent();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Appointments.DeleteAppointment, ApiEndpoints.V1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .HasPermission(Permissions.Policy(AppointmentsFeature.Permissions.Instance.Delete));
    }
}
