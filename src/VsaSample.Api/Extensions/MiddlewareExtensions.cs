using VsaSample.Api.Middlewares;

namespace VsaSample.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseRequestValidation(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestValidationMiddleware>();
        return app;
    }
}
