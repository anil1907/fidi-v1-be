namespace VsaSample.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseCors(this IServiceCollection services, IWebHostEnvironment env,
        CorsOptions corsOptions)
    {
        return services.AddCors(options =>
        {
            void ConfigurePolicy(Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder builder)
            {
                builder.WithOrigins(corsOptions!.AllowOrigins.ToArray() ?? [])
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }

            options.AddPolicy(CorsPolicies.Spa, ConfigurePolicy);
            options.AddDefaultPolicy(ConfigurePolicy);
        });
    }
}
