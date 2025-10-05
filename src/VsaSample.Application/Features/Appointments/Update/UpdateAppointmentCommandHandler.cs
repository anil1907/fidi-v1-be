namespace VsaSample.Application.Features.Appointments.Update;

internal sealed class UpdateAppointmentCommandHandler(
    IApplicationDbContext context,
    ILogger<UpdateAppointmentCommandHandler> logger)
    : ICommandHandler<UpdateAppointmentCommand>
{
    public async Task<Result> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (appointment is null)
        {
            return Result.Failure(AppointmentConstants.Errors.NotFound(request.Id));
        }

        if (!await context.Clients.AnyAsync(c => c.Id == request.ClientId, cancellationToken))
        {
            return Result.Failure(ClientConstants.Errors.NotFound(request.ClientId).Localize(request.Culture));
        }

        if (request.EndsAt <= request.StartsAt)
        {
            return Result.Failure(AppointmentConstants.Errors.InvalidTimeRange.Localize(request.Culture));
        }

        appointment.ClientId = request.ClientId;
        appointment.Title = request.Title.Trim();
        appointment.Description = request.Description;
        appointment.StartsAt = request.StartsAt;
        appointment.EndsAt = request.EndsAt;
        appointment.UpdateStatus(request.Status);

        if (request.IsActive.HasValue)
        {
            appointment.IsActive = request.IsActive.Value;
        }

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Appointment {AppointmentId} updated", appointment.Id);

        return Result.Success();
    }
}
