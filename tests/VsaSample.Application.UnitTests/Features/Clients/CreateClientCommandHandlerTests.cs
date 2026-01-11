using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using VsaSample.SharedKernel.Errors;
using VsaSample.Application.Features.Clients.Create;
using VsaSample.Domain.Entities;
using VsaSample.Infrastructure.Database.Application;
using Xunit;
using VsaSample.Application.UnitTests;

namespace VsaSample.Application.UnitTests.Features.Clients;

public class CreateClientCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateClient()
    {
        var organizationContext = new TestOrganizationContext(Guid.NewGuid());
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var context = new ApplicationDbContext(options, organizationContext);
        var handler = new CreateClientCommandHandler(context, NullLogger<CreateClientCommandHandler>.Instance);
        var command = new CreateClientCommand("Anıl", "Yıldırım", "anil@example.com", "+905555555555", "Initial notes", new[] { "Goal1", "Goal1", "Goal2" });

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        var saved = await context.Clients.SingleAsync();
        Assert.Equal(2, saved.Goals.Count);
        Assert.Contains("Goal1", saved.Goals);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenEmailExists()
    {
        var organizationContext = new TestOrganizationContext(Guid.NewGuid());
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var context = new ApplicationDbContext(options, organizationContext);
        context.Clients.Add(new Client
        {
            FirstName = "Existing",
            LastName = "Client",
            Email = "duplicate@example.com",
            Phone = "123",
            IsActive = true
        });
        await context.SaveChangesAsync();

        var handler = new CreateClientCommandHandler(context, NullLogger<CreateClientCommandHandler>.Instance);
        var command = new CreateClientCommand("Another", "Client", "duplicate@example.com", "456", null, null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }
}
