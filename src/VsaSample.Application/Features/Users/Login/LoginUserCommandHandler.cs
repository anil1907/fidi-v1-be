using VsaSample.Application.Abstractions.Authentication;
using Microsoft.Extensions.Hosting;

namespace VsaSample.Application.Features.Users.Login;

internal sealed class LoginUserCommandHandler(
    IApplicationDbContext context,
    ITokenProvider tokenProvider,
    ILdapManager ldapManager,
    IHostEnvironment environment) : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Username == command.Username, cancellationToken);

        if (user is null)
        {
            return Result<LoginUserResponse>.Failure(
                UserConstants.Errors.NotFoundByUsername.Localize(command.Culture));
        }

        if (environment.IsProduction())
        {
            var authenticated = await ldapManager.AuthenticateAsync(command.Username, command.Password, cancellationToken);
            if (!authenticated)
            {
                return Result<LoginUserResponse>.Failure(
                    UserConstants.Errors.Unauthorized().Localize(command.Culture));
            }
        }

        var token = tokenProvider.Create(user);

        var response = new LoginUserResponse(token,
            new UserDetailResponse(user.Username, user.Email, user.FirstName + "" + user.LastName, user.Id, user.Role));

        return Result<LoginUserResponse>.Success(response);
    }
}
