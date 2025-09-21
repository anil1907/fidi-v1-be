namespace VsaSample.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : Guid.Empty;
        // throw new ApplicationException("User id is unavailable");
    }
    
    public static string GetUserName(this ClaimsPrincipal? principal)
    {
        var username = principal?.FindFirstValue("Username");

        return
            username ?? ""; // TODO open this after allowing anonymous operationsthrow new ApplicationException("Username is unavailable");
    }
}
