namespace VsaSample.Application.Features.Users.Login;

public sealed record LoginUserResponse(string Token, UserDetailResponse User);

public sealed record UserDetailResponse(string Username, string Email, string FullName, long UserId, string Role);
