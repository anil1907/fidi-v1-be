namespace VsaSample.Infrastructure.Authentication;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public long UserId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ?? 0; // Its done for simplicity, ideally should throw exception if user is not authenticated

    public string Username =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserName() ?? ""; // Its done for simplicity, ideally should throw exception if user is not authenticated
}