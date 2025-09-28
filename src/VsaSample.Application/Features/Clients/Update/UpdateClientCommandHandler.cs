namespace VsaSample.Application.Features.Clients.Update;

internal sealed class UpdateClientCommandHandler(
    IApplicationDbContext context,
    ILogger<UpdateClientCommandHandler> logger)
    : ICommandHandler<UpdateClientCommand>
{
    public async Task<Result> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await context.Clients.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (client is null)
        {
            return Result.Failure(ClientConstants.Errors.NotFound(request.Id));
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        if (!client.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailInUse = await context.Clients
                .AnyAsync(c => c.Id != request.Id && c.Email.ToLower() == normalizedEmail, cancellationToken);
            if (emailInUse)
            {
                return Result.Failure(ClientConstants.Errors.EmailAlreadyExists.Localize(request.Culture));
            }
        }

        client.FirstName = request.FirstName.Trim();
        client.LastName = request.LastName.Trim();
        client.Email = request.Email.Trim();
        client.Phone = request.Phone.Trim();
        client.Notes = request.Notes;
        client.SetGoals(request.Goals);

        if (request.IsActive.HasValue)
        {
            client.IsActive = request.IsActive.Value;
        }

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Client {ClientId} updated", client.Id);

        return Result.Success();
    }
}
