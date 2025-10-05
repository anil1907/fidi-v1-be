namespace VsaSample.Application.Features.Appointments.GetById;

using VsaSample.Application.Features.Appointments.Shared;

public sealed record GetAppointmentByIdQuery(Guid Id) : IQuery<AppointmentResponse>;
