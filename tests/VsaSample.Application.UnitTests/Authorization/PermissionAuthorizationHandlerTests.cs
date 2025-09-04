using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VsaSample.Infrastructure.Authorization;
using Xunit;

namespace VsaSample.Application.UnitTests.Authorization;

public class PermissionAuthorizationHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenUserIsAuthenticated()
    {
        var requirement = new PermissionRequirement("test-permission");
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        var context = new AuthorizationHandlerContext(new[] { requirement }, user, null);

        var scopeFactory = new Mock<IServiceScopeFactory>();
        var handler = new PermissionAuthorizationHandler(scopeFactory.Object);

        await handler.HandleAsync(context);

        Assert.True(context.HasSucceeded);
    }
}
