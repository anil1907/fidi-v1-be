namespace VsaSample.Application.Features.Appointments.GetById;

using VsaSample.Application.Features.Appointments.Shared;

internal sealed class GetAppointmentByIdQueryHandler(
    IAppointmentRepository repository,
    ILogger<GetAppointmentByIdQueryHandler> logger)
    : IQueryHandler<GetAppointmentByIdQuery, AppointmentResponse>
{
    public async Task<Result<AppointmentResponse>> Handle(GetAppointmentByIdQuery query, CancellationToken cancellationToken)
    {
        var appointment = await repository.GetByIdAsync(query.Id, cancellationToken);
        if (appointment is null)
        {
            return Result<AppointmentResponse>.Failure(AppointmentConstants.Errors.NotFound(query.Id));
        }

        logger.LogInformation("Fetched appointment {AppointmentId}", query.Id);

        return Result<AppointmentResponse>.Success(appointment.ToResponse());
    }
}
