namespace VsaSample.Application.Features.Appointments.Shared;

public static class AppointmentMappings
{
    public static AppointmentResponse ToResponse(this Appointment appointment) => new()
    {
        Id = appointment.Id,
        ClientId = appointment.ClientId,
        Title = appointment.Title,
        Description = appointment.Description,
        StartsAt = appointment.StartsAt,
        EndsAt = appointment.EndsAt,
        Status = appointment.Status,
        IsActive = appointment.IsActive,
        CreateDate = appointment.CreateDate
    };
}
