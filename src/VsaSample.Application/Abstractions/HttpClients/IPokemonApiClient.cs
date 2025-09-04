using Refit;
using VsaSample.Application.Features.Pokemons.GetPokemons;

namespace VsaSample.Application.Abstractions.HttpClients;

public interface IPokemonApiClient
{
    [Get("/pokemon")]
    Task<PokemonListResponse> GetPokemonsAsync(
        [AliasAs("limit")] int limit,
        [AliasAs("offset")] int offset,
        CancellationToken cancellationToken = default);
}
