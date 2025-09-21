namespace VsaSample.Infrastructure.Authentication;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId
    {
        get
        {
            var principal = httpContextAccessor.HttpContext?.User;
            return principal?.GetUserId() ?? Guid.Empty;
        }
    }

    public string Username =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserName() ?? string.Empty;
}
