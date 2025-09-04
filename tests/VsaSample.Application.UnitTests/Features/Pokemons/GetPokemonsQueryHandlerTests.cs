using Bogus;
using Moq;
using VsaSample.Application.Abstractions.HttpClients;
using VsaSample.Application.Features.Pokemons.GetPokemons;
using Xunit;

namespace VsaSample.Application.UnitTests.Features.Pokemons;

public class GetPokemonsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPokemonListResponse_WhenClientReturnsData()
    {
        // Arrange
        var faker = new Faker();
        var items = new Faker<PokemonListItem>()
            .CustomInstantiator(f => new PokemonListItem(f.Name.ToString(), f.Internet.Url()))
            .Generate(3);
        var expected = new PokemonListResponse(items.Count, null, null, items);
        var mockClient = new Mock<IPokemonApiClient>();
        mockClient
            .Setup(c => c.GetPokemonsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var handler = new GetPokemonsQueryHandler(mockClient.Object);
        var query = new GetPokemonsQuery(3, 0);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value);
        mockClient.Verify(c => c.GetPokemonsAsync(3, 0, It.IsAny<CancellationToken>()), Times.Once);
    }
}
