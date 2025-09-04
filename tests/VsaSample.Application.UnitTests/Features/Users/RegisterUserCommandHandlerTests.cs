using Microsoft.EntityFrameworkCore;
using VsaSample.Application.Features.Users.Register;
using VsaSample.Infrastructure.Database.Application;
using Xunit;

namespace VsaSample.Application.UnitTests.Features.Users;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldRegisterUser()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var context = new ApplicationDbContext(options);
        var handler = new RegisterUserCommandHandler(context);
        var command = new RegisterUserCommand("john", "doe", "john@doe.com", "johnd", "Admin");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, context.Users.Count());
    }
}

