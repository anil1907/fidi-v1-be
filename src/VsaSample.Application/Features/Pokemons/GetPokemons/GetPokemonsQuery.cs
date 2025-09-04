namespace VsaSample.Application.Features.Pokemons.GetPokemons;

public sealed record GetPokemonsQuery(int Limit, int Offset) : IQuery<PokemonListResponse>;
