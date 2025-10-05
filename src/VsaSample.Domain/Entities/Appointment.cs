namespace VsaSample.Domain.Entities;

public sealed class Appointment : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;

    [Sieve(CanFilter = true, CanSort = true)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime StartsAt { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime EndsAt { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public AppointmentStatus Status { get; private set; } = AppointmentStatus.Scheduled;

    public void UpdateStatus(AppointmentStatus status) => Status = status;
}
