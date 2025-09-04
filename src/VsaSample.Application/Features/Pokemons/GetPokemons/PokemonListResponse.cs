using System.Text.Json.Serialization;

namespace VsaSample.Application.Features.Pokemons.GetPokemons;

public record PokemonListResponse(
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("next")] string? Next,
    [property: JsonPropertyName("previous")] string? Previous,
    [property: JsonPropertyName("results")] List<PokemonListItem> Results);

public record PokemonListItem(
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("url")] string Url);
