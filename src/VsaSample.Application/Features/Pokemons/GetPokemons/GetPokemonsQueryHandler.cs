using VsaSample.Application.Abstractions.HttpClients;

namespace VsaSample.Application.Features.Pokemons.GetPokemons;

public sealed class GetPokemonsQueryHandler(IPokemonApiClient client)
    : IQueryHandler<GetPokemonsQuery, PokemonListResponse>
{
    public async Task<Result<PokemonListResponse>> Handle(GetPokemonsQuery query, CancellationToken cancellationToken)
    {
        var response = await client.GetPokemonsAsync(query.Limit, query.Offset, cancellationToken);
        return Result<PokemonListResponse>.Success(response);
    }
}
