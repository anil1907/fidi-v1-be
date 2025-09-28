using VsaSample.Application.Features.Clients.Shared;

namespace VsaSample.Application.Features.Clients.Get;

public sealed record GetClientsPagedQuery(SieveModel Sieve, string? Search)
    : IQuery<PagedResult<ClientResponse>>;
