namespace VsaSample.Application.Features.Users.Register;

public sealed class RegisterUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher<User> passwordHasher)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Email == command.Email || u.Username.Equals(command.Username), cancellationToken))
        {
            return Result<Guid>.Failure(
                UserConstants.Errors.EmailNotUnique.Localize(command.Culture));
        }

        var user = new User
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Username = command.Username
        };

        user.SetRole(command.Role);
        var passwordHash = passwordHasher.HashPassword(user, command.Password);
        user.SetPasswordHash(passwordHash);

        context.Users.Add(user);

        await context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}
