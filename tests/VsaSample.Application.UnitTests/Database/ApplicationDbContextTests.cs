using Microsoft.EntityFrameworkCore;
using VsaSample.Infrastructure.Database.Application;
using Xunit;
using VsaSample.Application.UnitTests;

namespace VsaSample.Application.UnitTests.Database;

public class ApplicationDbContextTests
{
    [Fact]
    public void OnModelCreating_ShouldSetDefaultSchema()
    {
        var organizationContext = new TestOrganizationContext(Guid.NewGuid());
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("schema-db")
            .Options;

        using var context = new ApplicationDbContext(options, organizationContext);

        Assert.Equal("public", context.Model.GetDefaultSchema());
    }
}
