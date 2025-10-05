namespace VsaSample.Application.Features.Appointments.Delete;

public sealed record DeleteAppointmentCommand(Guid Id) : ICommand;
