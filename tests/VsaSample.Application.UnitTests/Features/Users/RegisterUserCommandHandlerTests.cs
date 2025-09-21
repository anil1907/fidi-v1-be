using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VsaSample.Application.Features.Users.Register;
using VsaSample.Domain.Entities;
using VsaSample.Domain.Enums;
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
        var handler = new RegisterUserCommandHandler(context, new PasswordHasher<User>());
        var command = new RegisterUserCommand("john@doe.com", "john", "doe", "johnd", "P@ssw0rd!", UserRole.Admin);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        var savedUser = await context.Users.SingleAsync();
        Assert.Equal(UserRole.Admin, savedUser.Role);
        Assert.NotEqual(command.Password, savedUser.PasswordHash);
        Assert.False(string.IsNullOrWhiteSpace(savedUser.PasswordHash));
    }
}

