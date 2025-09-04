using VsaSample.Application.Features.Pokemons.GetPokemons;

namespace VsaSample.Api.Endpoints.v1;

public class Pokemons : EndpointGroupBase
{
    public override void Map(IEndpointRouteBuilder app)
    {
        var route = MapGroup(app)
            .MapToApiVersion(ApiEndpoints.V1);

        route.MapGet("{offset:int}/{limit:int}",
                async (int offset, int limit, IQueryHandler<GetPokemonsQuery, PokemonListResponse> handler, CancellationToken ct) =>
                    (await handler.Handle(new GetPokemonsQuery(offset, limit), ct)).ToHttpResponse())
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Pokemons.GetPokemons, ApiEndpoints.V1))
            .Produces<PokemonListResponse>()
            .Produces(StatusCodes.Status400BadRequest);
    }
}
