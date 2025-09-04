using VsaSample.Application;
using HealthChecks.UI.Client;
using VsaSample.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog.Debugging;
using VsaSample.Api;

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

var jwtOptions = builder.Configuration.GetSection(ConfigSections.Jwt).Get<JwtOptions>()!;
var redisOptions = builder.Configuration.GetSection(ConfigSections.Redis).Get<RedisOptions>()!;
var corsOptions = builder.Configuration.GetSection(ConfigSections.Cors).Get<CorsOptions>()!;

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(jwtOptions, builder.Configuration.GetConnectionString("ApplicationConnection")!)
    .UseCors(builder.Environment, corsOptions)
    .AddCaching(redisOptions);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestContextLogging();
app.UseRequestValidation();
app.MapEndpoints();
if (!app.Environment.IsProduction())
    app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsProduction())
    app.UseSwaggerWithUi();

await app.RunAsync();

