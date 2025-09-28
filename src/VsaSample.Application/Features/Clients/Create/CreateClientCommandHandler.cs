namespace VsaSample.Application.Features.Clients.Create;

public sealed class CreateClientCommandHandler(
    IApplicationDbContext context,
    ILogger<CreateClientCommandHandler> logger)
    : ICommandHandler<CreateClientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        if (await context.Clients.AnyAsync(c => c.Email.ToLower() == normalizedEmail, cancellationToken))
        {
            return Result<Guid>.Failure(ClientConstants.Errors.EmailAlreadyExists.Localize(request.Culture));
        }

        var client = new Client
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim(),
            Phone = request.Phone.Trim(),
            Notes = request.Notes,
            IsActive = true
        };
        client.SetGoals(request.Goals);

        context.Clients.Add(client);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Client {ClientId} created", client.Id);

        return Result<Guid>.Success(client.Id);
    }
}
