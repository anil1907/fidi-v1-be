namespace VsaSample.Application.Features.Appointments.Delete;

internal sealed class DeleteAppointmentCommandHandler(
    IApplicationDbContext context,
    ILogger<DeleteAppointmentCommandHandler> logger)
    : ICommandHandler<DeleteAppointmentCommand>
{
    public async Task<Result> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await context.Appointments.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (appointment is null)
        {
            return Result.Failure(AppointmentConstants.Errors.NotFound(request.Id));
        }

        context.Appointments.Remove(appointment);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Appointment {AppointmentId} deleted", request.Id);

        return Result.Success();
    }
}
