namespace VsaSample.Application.Features.Appointments.Create;

public sealed record CreateAppointmentCommand(
    Guid ClientId,
    string Title,
    string? Description,
    DateTime StartsAt,
    DateTime EndsAt,
    AppointmentStatus Status) : ICommand<Guid>
{
    public string Culture { get; init; } = Cultures.Default;
}
