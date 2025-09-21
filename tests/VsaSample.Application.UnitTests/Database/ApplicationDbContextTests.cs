using Microsoft.EntityFrameworkCore;
using VsaSample.Infrastructure.Database.Application;
using Xunit;

namespace VsaSample.Application.UnitTests.Database;

public class ApplicationDbContextTests
{
    [Fact]
    public void OnModelCreating_ShouldSetDefaultSchema()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("schema-db")
            .Options;

        using var context = new ApplicationDbContext(options);

        Assert.Equal("public", context.Model.GetDefaultSchema());
    }
}
