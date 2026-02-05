namespace VsaSample.Api.Contracts.Auth;

public sealed record ExchangeRequest(string Code, string CodeVerifier, string RedirectUri);
