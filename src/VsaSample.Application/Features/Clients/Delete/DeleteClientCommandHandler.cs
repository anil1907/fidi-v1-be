namespace VsaSample.Application.Features.Clients.Delete;

internal sealed class DeleteClientCommandHandler(
    IApplicationDbContext context,
    ILogger<DeleteClientCommandHandler> logger)
    : ICommandHandler<DeleteClientCommand>
{
    public async Task<Result> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var client = await context.Clients.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (client is null)
        {
            return Result.Failure(ClientConstants.Errors.NotFound(request.Id));
        }

        context.Clients.Remove(client);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Client {ClientId} deleted", client.Id);

        return Result.Success();
    }
}
