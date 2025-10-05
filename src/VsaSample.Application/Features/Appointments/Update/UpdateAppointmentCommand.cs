namespace VsaSample.Application.Features.Appointments.Update;

public sealed record UpdateAppointmentCommand(
    Guid Id,
    Guid ClientId,
    string Title,
    string? Description,
    DateTime StartsAt,
    DateTime EndsAt,
    AppointmentStatus Status,
    bool? IsActive) : ICommand
{
    public string Culture { get; init; } = Cultures.Default;
}

public sealed record UpdateAppointmentRequest(
    Guid ClientId,
    string Title,
    string? Description,
    DateTime StartsAt,
    DateTime EndsAt,
    AppointmentStatus Status,
    bool? IsActive);
