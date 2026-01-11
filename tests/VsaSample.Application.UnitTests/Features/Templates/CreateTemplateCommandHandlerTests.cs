using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using VsaSample.Application.Abstractions.Caching;
using VsaSample.Application.Features.Templates.Create;
using VsaSample.Domain.Entities.Templates;
using VsaSample.Infrastructure.Database;
using VsaSample.Infrastructure.Database.Application;
using VsaSample.SharedKernel.Errors;
using Xunit;
using VsaSample.Application.UnitTests;

namespace VsaSample.Application.UnitTests.Features.Templates;

public class CreateTemplateCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateTemplate()
    {
        var organizationContext = new TestOrganizationContext(Guid.NewGuid());
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var context = new ApplicationDbContext(options, organizationContext);
        var repository = new TemplateRepository(context, new NoOpCacheService());

        var sections = new[]
        {
            new TemplateSectionDto("section-1", "Breakfast", new[]
            {
                new TemplateSectionItemDto("item-1", "Oatmeal", "1 bowl", null, 350)
            })
        };

        var handler = new CreateTemplateCommandHandler(repository, NullLogger<CreateTemplateCommandHandler>.Instance);
        var command = new CreateTemplateCommand("Sample", "Daily template", sections);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        var saved = await context.Templates.SingleAsync();
        Assert.Equal("Sample", saved.Name);
        Assert.True(saved.IsActive);
        Assert.Single(saved.Sections);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenNameDuplicate()
    {
        var organizationContext = new TestOrganizationContext(Guid.NewGuid());
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var context = new ApplicationDbContext(options, organizationContext);
        context.Templates.Add(new Template("Sample", null));
        await context.SaveChangesAsync();
        var repository = new TemplateRepository(context, new NoOpCacheService());

        var handler = new CreateTemplateCommandHandler(repository, NullLogger<CreateTemplateCommandHandler>.Instance);
        var command = new CreateTemplateCommand("Sample", null, Array.Empty<TemplateSectionDto>());

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }

    private sealed class NoOpCacheService : ICacheService
    {
        public ValueTask<T?> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T?>> factory, CancellationToken cancellationToken = default)
            => factory(cancellationToken);

        public ValueTask SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
            => ValueTask.CompletedTask;

        public ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default)
            => ValueTask.CompletedTask;
    }
}
