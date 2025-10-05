namespace VsaSample.Application.Features.Appointments.Create;

public sealed class CreateAppointmentCommandHandler(
    IApplicationDbContext context,
    ILogger<CreateAppointmentCommandHandler> logger)
    : ICommandHandler<CreateAppointmentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var clientExists = await context.Clients.AnyAsync(c => c.Id == request.ClientId, cancellationToken);
        if (!clientExists)
        {
            return Result<Guid>.Failure(ClientConstants.Errors.NotFound(request.ClientId).Localize(request.Culture));
        }

        if (request.EndsAt <= request.StartsAt)
        {
            return Result<Guid>.Failure(AppointmentConstants.Errors.InvalidTimeRange.Localize(request.Culture));
        }

        var appointment = new Appointment
        {
            ClientId = request.ClientId,
            Title = request.Title.Trim(),
            Description = request.Description,
            StartsAt = request.StartsAt,
            EndsAt = request.EndsAt,

            IsActive = true
        };

        appointment.UpdateStatus(request.Status);
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Appointment {AppointmentId} created for client {ClientId}", appointment.Id, appointment.ClientId);

        return Result<Guid>.Success(appointment.Id);
    }
}
