namespace VsaSample.Api.Extensions;

public static class ServiceCollectionExtensions
{
    
    public static IServiceCollection UseCors(this IServiceCollection services, IWebHostEnvironment env,
        CorsOptions corsOptions)
    {
        if (env.IsProduction())
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(corsOptions!.AllowOrigins.ToArray() ?? [])
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
        else
        {
            services.AddCors();
        }

        return services;
    }
    
}