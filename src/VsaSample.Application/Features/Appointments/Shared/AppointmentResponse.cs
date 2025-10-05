namespace VsaSample.Application.Features.Appointments.Shared;

public sealed record AppointmentResponse
{
    public Guid Id { get; init; }
    public Guid ClientId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime StartsAt { get; init; }
    public DateTime EndsAt { get; init; }
    public AppointmentStatus Status { get; init; }
    public bool IsActive { get; init; }
    public DateTime? CreateDate { get; init; }
}
