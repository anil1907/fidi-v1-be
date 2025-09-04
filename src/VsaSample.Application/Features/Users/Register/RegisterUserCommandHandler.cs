namespace VsaSample.Application.Features.Users.Register;

public sealed class RegisterUserCommandHandler(IApplicationDbContext context)
    : ICommandHandler<RegisterUserCommand, long>
{
    public async Task<Result<long>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Email == command.Email || u.Username.Equals(command.Username), cancellationToken))
        {
            return Result<long>.Failure(
                UserConstants.Errors.EmailNotUnique.Localize(command.Culture));
        }

        var user = new User
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Username = command.Username,
            Role = command.Role
        };

        context.Users.Add(user);

        await context.SaveChangesAsync(cancellationToken);

        return Result<long>.Success(user.Id);
    }
}
