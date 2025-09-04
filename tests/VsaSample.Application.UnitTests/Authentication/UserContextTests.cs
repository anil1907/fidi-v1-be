using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using VsaSample.Infrastructure.Authentication;
using Xunit;

namespace VsaSample.Application.UnitTests.Authentication;

public class UserContextTests
{
    [Fact]
    public void ShouldReturnUserDataFromClaims()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "5"),
            new Claim("Username", "tester")
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var httpContext = new DefaultHttpContext { User = principal };
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        var context = new UserContext(accessor);

        Assert.Equal(5, context.UserId);
        Assert.Equal("tester", context.Username);
    }
}

