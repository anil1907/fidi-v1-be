using VsaSample.Application.Features.Clients.Shared;

namespace VsaSample.Application.Features.Clients.GetById;

internal sealed class GetClientByIdQueryHandler(
    IClientRepository repository,
    ILogger<GetClientByIdQueryHandler> logger)
    : IQueryHandler<GetClientByIdQuery, ClientResponse>
{
    public async Task<Result<ClientResponse>> Handle(GetClientByIdQuery query, CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdAsync(query.Id, cancellationToken);
        if (client is null)
        {
            return Result<ClientResponse>.Failure(ClientConstants.Errors.NotFound(query.Id));
        }

        logger.LogInformation("Fetched client {ClientId}", query.Id);

        return Result<ClientResponse>.Success(client.ToResponse());
    }
}
