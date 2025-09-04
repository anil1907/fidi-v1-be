using Refit;
using VsaSample.Application.Abstractions.HttpClients;
using Xunit;

namespace VsaSample.Api.IntegrationTests;

public class PokemonApiClientTests
{
    [Fact]
    public async Task GetPokemonsAsync_ShouldReturnResults()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("https://pokeapi.co/api/v2") };
        var client = RestService.For<IPokemonApiClient>(httpClient);

        var response = await client.GetPokemonsAsync(2, 0, CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal(2, response.Results.Count);
    }

    [Fact]
    public async Task GetPokemonsAsync_WithOffset_ShouldReturnShiftedResults()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("https://pokeapi.co/api/v2") };
        var client = RestService.For<IPokemonApiClient>(httpClient);

        var response = await client.GetPokemonsAsync(1, 1, CancellationToken.None);

        Assert.NotNull(response);
        Assert.Single(response.Results);
        Assert.Equal("ivysaur", response.Results[0].Name);
    }
}
