using Microsoft.AspNetCore.Authentication.JwtBearer;
using StackExchange.Redis;
using VsaSample.Api.Handlers;
using VsaSample.Api.OpenApi;

namespace VsaSample.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
        services
            .AddOpenApi()
            .AddSwaggerGen(c =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lowercase
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, Array.Empty<string>() }
                });
            });

        services.ConfigureOptions<ConfigureSwaggerGenOptions>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static IServiceCollection AddCaching(this IServiceCollection services, RedisOptions redisOptions)
    {
        var configOptions = new ConfigurationOptions
        {
            Password = redisOptions!.Password!,
            Ssl = redisOptions!.Ssl,
            ConnectTimeout = redisOptions.ConnectTimeout,
            ConnectRetry = redisOptions.ConnectRetry,
            DefaultDatabase = redisOptions.Database
        };

        foreach (var host in redisOptions.Hosts)
        {
            configOptions.EndPoints.Add(host.Host, host.Port);
        }

        services.AddHybridCache();

        return services;
    }
}