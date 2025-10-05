using Microsoft.EntityFrameworkCore;
using VsaSample.Domain.Entities;
using VsaSample.Domain.Enums;
using VsaSample.SharedKernel.Errors;
using Microsoft.Extensions.Logging.Abstractions;
using VsaSample.Application.Features.Appointments.Create;
using VsaSample.Infrastructure.Database.Application;
using Xunit;

namespace VsaSample.Application.UnitTests.Features.Appointments;

public class CreateAppointmentCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateAppointment()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var context = new ApplicationDbContext(options);
        var clientId = Guid.NewGuid();
        context.Clients.Add(new Client { Id = clientId, FirstName = "Test", LastName = "Client", Email = "test@example.com", Phone = "123", IsActive = true });
        await context.SaveChangesAsync();

        var handler = new CreateAppointmentCommandHandler(context, NullLogger<CreateAppointmentCommandHandler>.Instance);
        var command = new CreateAppointmentCommand(clientId, "Therapy", "Discuss progress", DateTime.UtcNow, DateTime.UtcNow.AddHours(1), AppointmentStatus.Scheduled);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        var saved = await context.Appointments.SingleAsync();
        Assert.Equal(AppointmentStatus.Scheduled, saved.Status);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEndBeforeStart()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var context = new ApplicationDbContext(options);
        var clientId = Guid.NewGuid();
        context.Clients.Add(new Client { Id = clientId, FirstName = "Test", LastName = "Client", Email = "test@example.com", Phone = "123", IsActive = true });
        await context.SaveChangesAsync();

        var handler = new CreateAppointmentCommandHandler(context, NullLogger<CreateAppointmentCommandHandler>.Instance);
        var start = DateTime.UtcNow;
        var command = new CreateAppointmentCommand(clientId, "Therapy", null, start, start.AddMinutes(-30), AppointmentStatus.Scheduled);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Problem, result.Error.Type);
    }
}
