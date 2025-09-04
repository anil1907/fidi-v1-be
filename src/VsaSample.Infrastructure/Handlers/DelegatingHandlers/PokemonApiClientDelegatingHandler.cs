using Microsoft.Extensions.Logging;

namespace VsaSample.Infrastructure.Handlers.DelegatingHandlers;

internal sealed class PokemonApiClientDelegatingHandler(ILogger<PokemonApiClientDelegatingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching pokemons");
            throw;
        }
    }
}
