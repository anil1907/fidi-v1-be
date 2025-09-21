using Moq;
using VsaSample.Application.Features.Categories.Create;
using VsaSample.Application.Abstractions.Repositories;
using VsaSample.Domain.Entities;
using Microsoft.Extensions.Logging;
using Xunit;

namespace VsaSample.Application.UnitTests.Features.Categories;

public class CreateCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateCategory()
    {
        var repository = new Mock<ICategoryRepository>();
        var newId = Guid.NewGuid();
        repository
            .Setup(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newId);
        var logger = new Mock<ILogger<CreateCategoryCommandHandler>>();
        var handler = new CreateCategoryCommandHandler(repository.Object, logger.Object);
        var command = new CreateCategoryCommand("NewCat");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(newId, result.Value);
        repository.Verify(r => r.AddAsync(It.Is<Category>(c => c.Name == "NewCat"), It.IsAny<CancellationToken>()), Times.Once);
    }
}

