using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using VsaSample.Api.Contracts.Auth;
using VsaSample.Api.Options;

namespace VsaSample.Api.Authentication;

public sealed class KeycloakTokenClient(IHttpClientFactory httpClientFactory, IOptions<KeycloakOptions> options)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Keycloak");
    private readonly KeycloakOptions _options = options.Value;

    public async Task<TokenResponse> ExchangeAuthorizationCodeAsync(
        ExchangeRequest request,
        CancellationToken cancellationToken)
    {
        var content = new FormUrlEncodedContent(BuildTokenForm(request));
        using var response = await _httpClient.PostAsync(GetTokenEndpoint(), content, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new KeycloakTokenExchangeException(response.StatusCode, errorBody);
        }

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken);
        if (tokenResponse is null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
        {
            throw new KeycloakTokenExchangeException(HttpStatusCode.BadGateway, "Token response was empty.");
        }

        return tokenResponse;
    }

    private string GetTokenEndpoint() =>
        $"realms/{_options.Realm}/protocol/openid-connect/token";

    private IReadOnlyCollection<KeyValuePair<string, string>> BuildTokenForm(ExchangeRequest request)
    {
        var form = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "authorization_code"),
            new("client_id", _options.ClientId),
            new("code", request.Code),
            new("redirect_uri", request.RedirectUri),
            new("code_verifier", request.CodeVerifier)
        };

        if (!string.IsNullOrWhiteSpace(_options.ClientSecret))
        {
            form.Add(new KeyValuePair<string, string>("client_secret", _options.ClientSecret));
        }

        return form;
    }
}

public sealed class KeycloakTokenExchangeException(HttpStatusCode statusCode, string? errorBody)
    : Exception(errorBody)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public string? ErrorBody { get; } = errorBody;
}
