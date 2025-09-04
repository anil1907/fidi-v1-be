using VsaSample.Infrastructure.Options;
using Xunit;

namespace VsaSample.Application.UnitTests.Options;

public class OptionsTests
{
    [Fact]
    public void FtpOptions_ShouldHaveDefaults()
    {
        var opt = new FtpOptions();
        Assert.Equal(21, opt.Port);
        Assert.Equal(string.Empty, opt.Host);
    }

    [Fact]
    public void JwtOptions_ShouldStoreValues()
    {
        var opt = new JwtOptions { Issuer = "i", Audience = "a", Secret = "s", ExpirationInMinutes = 5 };
        Assert.Equal("i", opt.Issuer);
        Assert.Equal(5, opt.ExpirationInMinutes);
    }

    [Fact]
    public void RedisOptions_ShouldInitializeHosts()
    {
        var opt = new RedisOptions();
        Assert.NotNull(opt.Hosts);
        Assert.Empty(opt.Hosts);
    }
}

