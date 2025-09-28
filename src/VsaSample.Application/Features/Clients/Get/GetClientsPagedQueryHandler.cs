namespace VsaSample.Application.Features.Clients.Get;

using Microsoft.EntityFrameworkCore;
using VsaSample.Application.Features.Clients.Shared;

internal sealed class GetClientsPagedQueryHandler(
    IClientRepository repository,
    ISieveProcessor sieve,
    ILogger<GetClientsPagedQueryHandler> logger)
    : IQueryHandler<GetClientsPagedQuery, PagedResult<ClientResponse>>
{
    public async Task<Result<PagedResult<ClientResponse>>> Handle(GetClientsPagedQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Client> clients = repository.Query;

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            var loweredPattern = $"%{search.ToLowerInvariant()}%";
            clients = clients.Where(c =>
                EF.Functions.Like(c.FirstName.ToLower(), loweredPattern) ||
                EF.Functions.Like(c.LastName.ToLower(), loweredPattern) ||
                EF.Functions.Like(c.Email.ToLower(), loweredPattern) ||
                EF.Functions.Like(c.Phone.ToLower(), loweredPattern));
        }

        var filtered = sieve.Apply(query.Sieve, clients, applyPagination: false);
        var total = await filtered.CountAsync(cancellationToken);

        var paged = sieve.Apply(query.Sieve, filtered, applyPagination: true);
        var items = await paged
            .Select(c => c.ToResponse())
            .ToListAsync(cancellationToken);

        var pageSize = query.Sieve.PageSize ?? 10;
        var pageNumber = query.Sieve.Page ?? 1;
        var payload = PagedResult<ClientResponse>.Create(items, pageNumber, pageSize, total);

        logger.LogInformation(
            "Fetched clients page {Page}/{Size} (Total: {Total})",
            pageNumber,
            pageSize,
            total);

        return Result<PagedResult<ClientResponse>>.Success(payload);
    }
}
