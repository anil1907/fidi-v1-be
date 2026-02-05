using VsaSample.Application;
using HealthChecks.UI.Client;
using VsaSample.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog.Debugging;
using VsaSample.Api;
using Microsoft.AspNetCore.Http.Json;
using VsaSample.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using VsaSample.Api.Authentication;
using VsaSample.Api.Options;
using System.Security.Claims;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

SelfLog.Enable(Console.Error);
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
);  

builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection(ConfigSections.Redis));
builder.Services.Configure<CorsOptions>(builder.Configuration.GetSection(ConfigSections.Cors));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(ConfigSections.Jwt));
builder.Services.Configure<DbInterceptorOptions>(builder.Configuration.GetSection(ConfigSections.Database.Interceptors));
builder.Services.Configure<LdapOptions>(builder.Configuration.GetSection(ConfigSections.Ldap));
builder.Services.Configure<FtpOptions>(builder.Configuration.GetSection(ConfigSections.Ftp));
builder.Services.Configure<KeycloakOptions>(builder.Configuration.GetSection(ConfigSections.Keycloak));
builder.Services.Configure<SpaOptions>(builder.Configuration.GetSection(ConfigSections.Spa));

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var jwtOptions = builder.Configuration.GetSection(ConfigSections.Jwt).Get<JwtOptions>()!;
var redisOptions = builder.Configuration.GetSection(ConfigSections.Redis).Get<RedisOptions>()!;
var corsOptions = builder.Configuration.GetSection(ConfigSections.Cors).Get<CorsOptions>()!;
var keycloakOptions = builder.Configuration.GetSection(ConfigSections.Keycloak).Get<KeycloakOptions>()!;

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(jwtOptions, builder.Configuration.GetConnectionString("ApplicationConnection")!)
    .UseCors(builder.Environment, corsOptions)
    .AddCaching(redisOptions);

builder.Services.AddControllers();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddHttpClient("Keycloak", (sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<KeycloakOptions>>().Value;
    var baseUrl = options.BaseUrl.TrimEnd('/');
    if (!string.IsNullOrWhiteSpace(baseUrl))
    {
        client.BaseAddress = new Uri($"{baseUrl}/");
    }
});

builder.Services.AddScoped<KeycloakTokenClient>();
builder.Services.AddScoped<KeycloakTokenValidator>();

var authority = $"{keycloakOptions.BaseUrl.TrimEnd('/')}/realms/{keycloakOptions.Realm}";
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = AuthConstants.KeycloakScheme;
        options.DefaultChallengeScheme = AuthConstants.KeycloakScheme;
    })
    .AddJwtBearer(AuthConstants.KeycloakScheme, options =>
    {
        options.Authority = authority;
        options.RequireHttpsMetadata = false;
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authority,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            ValidateAudience = true,
            // Accept aud or azp = client_id for Keycloak SPA tokens while still validating issuer/signature.
            AudienceValidator = (audiences, token, _) =>
            {
                if (string.IsNullOrWhiteSpace(keycloakOptions.ClientId))
                {
                    return true;
                }

                if (audiences is not null &&
                    audiences.Any(a => string.Equals(a, keycloakOptions.ClientId, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }

                if (token is JwtSecurityToken jwt)
                {
                    var azp = jwt.Claims.FirstOrDefault(claim => claim.Type == "azp")?.Value;
                    return string.Equals(azp, keycloakOptions.ClientId, StringComparison.OrdinalIgnoreCase);
                }

                return false;
            }
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (string.IsNullOrWhiteSpace(context.Token) &&
                    context.Request.Cookies.TryGetValue(AuthConstants.AccessTokenCookieName, out var cookieToken))
                {
                    context.Token = cookieToken;
                }

                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                if (context.Principal?.Identity is ClaimsIdentity identity)
                {
                    KeycloakClaimsMapper.Map(identity);
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

await app.ApplyMigrationsAsync();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestContextLogging();
app.UseRequestValidation();
app.MapEndpoints();
app.MapControllers();
if (!app.Environment.IsProduction())
    app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseCors(CorsPolicies.Spa);
app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsProduction())
    app.UseSwaggerWithUi();

await app.RunAsync();

