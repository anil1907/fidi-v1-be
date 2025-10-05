namespace VsaSample.Application.Features.Appointments.Get;

using VsaSample.Application.Features.Appointments.Shared;

public sealed record GetAppointmentsPagedQuery(
    SieveModel Sieve,
    string? Search,
    Guid? ClientId,
    AppointmentStatus? Status,
    DateTime? StartsAfter,
    DateTime? EndsBefore)
    : IQuery<PagedResult<AppointmentResponse>>;
