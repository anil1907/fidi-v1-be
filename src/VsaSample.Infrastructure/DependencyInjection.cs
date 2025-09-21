using VsaSample.Infrastructure.Authorization;
using VsaSample.Infrastructure.Caching;
using VsaSample.Infrastructure.Database;
using VsaSample.Infrastructure.Database.Interceptors;
using VsaSample.Infrastructure.Excel;
using VsaSample.Infrastructure.FileTransfer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Refit;
using Sieve.Services;
using VsaSample.Application.Abstractions.HttpClients;
using VsaSample.Infrastructure.Handlers.DelegatingHandlers;

namespace VsaSample.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, JwtOptions jwtOptions, string connectionString) =>
        services
            .AddServices()
            .AddHttpClients()
            .AddDatabase(connectionString)
            .AddHealthCheck(connectionString)
            .AddAuthenticationInternal(jwtOptions)
            .AddAuthorizationInternal();

    private static IServiceCollection AddDatabase(this IServiceCollection services,string connectionString)
    {

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            // options.UseInMemoryDatabase("AppDb");
            options.UseNpgsql(connectionString);
            // options.UseSqlServer(configuration.GetConnectionString("ApplicationConnection")!, sqlServerOptions =>
            //     sqlServerOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default));

            options.AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
            options.AddInterceptors(sp.GetRequiredService<SlowCommandInterceptor>());
        });
        
        
        
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
    
    private static IServiceCollection AddHealthCheck(this IServiceCollection services, string connectionString)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(connectionString);

        services.Scan(scan => scan.FromAssembliesOf(typeof(SieveProcessor))
            .AddClasses(classes => classes.AssignableTo(typeof(ISieveProcessor)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }
    
    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services, JwtOptions jwtOptions)
    {
        var secretKey = jwtOptions.Secret;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        
        services
            .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = securityKey,
                        ValidAudience = jwtOptions.Audience,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

        services.AddHttpContextAccessor();
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IUserContext)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ITokenProvider)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<ICacheService, CacheService>()
            .AddScoped<IExcelHelper, ExcelHelper>()
            .AddScoped<ILdapManager, LdapManager>()
            .AddScoped<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<IFileTransferHelper, FtpHelper>();

        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(DbCommandInterceptor)), publicOnly: false)
            .AsSelf()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(SaveChangesInterceptor)), publicOnly: false)
            .AsSelf()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddTransient<PokemonApiClientDelegatingHandler>();

        services
            .AddRefitClient<IPokemonApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://pokeapi.co/api/v2"))
            .AddHttpMessageHandler<PokemonApiClientDelegatingHandler>()
            .AddStandardResilienceHandler();

        return services;
    }
    
    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(PermissionProvider)), publicOnly: false)
            .AsSelf()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IAuthorizationHandler)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithTransientLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IAuthorizationPolicyProvider)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        return services;
    }
}
