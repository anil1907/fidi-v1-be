using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VsaSample.Application.Features.Users.Register;
using VsaSample.Domain.Entities;
using VsaSample.Domain.Enums;
using VsaSample.Infrastructure.Database.Application;
using Xunit;
using VsaSample.Application.UnitTests;

namespace VsaSample.Application.UnitTests.Features.Users;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldRegisterUser()
    {
        var organizationContext = new TestOrganizationContext(Guid.NewGuid());
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var context = new ApplicationDbContext(options, organizationContext);
        var handler = new RegisterUserCommandHandler(context, new PasswordHasher<User>());
        var command = new RegisterUserCommand("john@doe.com", "john", "doe", "johnd", "P@ssw0rd!", UserRole.OrgAdmin);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        var savedUser = await context.Users.SingleAsync();
        Assert.Equal(UserRole.OrgAdmin, savedUser.Role);
        Assert.NotEqual(command.Password, savedUser.PasswordHash);
        Assert.False(string.IsNullOrWhiteSpace(savedUser.PasswordHash));
    }
}

