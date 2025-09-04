namespace VsaSample.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static long GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return long.TryParse(userId, out var parsedUserId) ? parsedUserId : 0;
        // throw new ApplicationException("User id is unavailable");
    }
    
    public static string GetUserName(this ClaimsPrincipal? principal)
    {
        var username = principal?.FindFirstValue("Username");

        return
            username ?? ""; // TODO open this after allowing anonymous operationsthrow new ApplicationException("Username is unavailable");
    }
}