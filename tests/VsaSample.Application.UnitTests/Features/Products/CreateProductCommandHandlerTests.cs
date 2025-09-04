using Moq;
using VsaSample.Application.Abstractions.Repositories;
using VsaSample.Application.Features.Products.Create;
using VsaSample.Domain.Entities;
using Microsoft.Extensions.Logging;
using Xunit;

namespace VsaSample.Application.UnitTests.Features.Products;

public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateProduct()
    {
        var repository = new Mock<IProductRepository>();
        repository
            .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        var logger = new Mock<ILogger<CreateProductCommandHandler>>();
        var handler = new CreateProductCommandHandler(repository.Object, logger.Object);
        var command = new CreateProductCommand("sku", 10m, true, "en", new List<CreateProductTranslation>());

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
        repository.Verify(r => r.AddAsync(It.Is<Product>(p => p.Sku == "sku"), It.IsAny<CancellationToken>()), Times.Once);
    }
}

