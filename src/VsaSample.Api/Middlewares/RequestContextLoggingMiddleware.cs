using VsaSample.Application.Abstractions.Authentication;
using Serilog.Context;
using System.Text;

namespace VsaSample.Api.Middlewares;

public class RequestContextLoggingMiddleware(RequestDelegate next)
{
    private static readonly PathString LoginPath = new("/api/users/login");

    public async Task Invoke(HttpContext context, IUserContext userContext)
    {
        (string Name, object Value)[] logProperties =
        [
            ("ClientIp", GetClientIp(context)),
            ("UserAgent", GetHeaderValue(context, "User-Agent", "n/a")),
            ("AcceptLanguage", GetHeaderValue(context, "Accept-Language", "n/a")),
            ("Referer", GetHeaderValue(context, "Referer", "n/a")),
            ("Username", userContext.Username),
            ("UserId", userContext.UserId),
            ("HttpMethod", GetHttpMethod(context)),
            ("Path", GetPath(context))
        ];

        using (PushLogContextProperties(logProperties))
        {
            var isLoginPath = context.Request.Path.Equals(LoginPath, StringComparison.OrdinalIgnoreCase);

            string? requestBody = null;
            Stream? originalBody = null;
            MemoryStream? newBody = null;

            if (!isLoginPath)
            {
                context.Request.EnableBuffering();

                if (context.Request.ContentLength > 0)
                {
                    requestBody = await ReadBodyAsync(context.Request.Body);
                }

                context.Request.Body.Position = 0;

                originalBody = context.Response.Body;
                newBody = new MemoryStream();
                context.Response.Body = newBody;
            }

            await next(context);

            string? responseBody = null;

            if (!isLoginPath)
            {
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                if (context.Response.Body.Length > 0)
                {
                    responseBody = await ReadBodyAsync(context.Response.Body);
                }

                context.Response.Body.Seek(0, SeekOrigin.Begin);

                await newBody!.CopyToAsync(originalBody!);
            }

            var additionalProperties = new List<(string Name, object Value)>
            {
                ("StatusCode", GetStatusCode(context))
            };

            if (requestBody != null)
            {
                additionalProperties.Add(("RequestBody", requestBody));
            }

            if (responseBody != null)
            {
                additionalProperties.Add(("ResponseBody", responseBody));
            }

            using (PushLogContextProperties(additionalProperties.ToArray()))
            {
                Log.Information("HTTP {Method} {Path}", context.Request.Method, context.Request.Path);
            }
        }
    }

    private static string GetHeaderValue(HttpContext context, string headerName, string? fallback)
    {
        context.Request.Headers.TryGetValue(headerName, out var values);
        return values.FirstOrDefault() ?? fallback ?? "n/a";
    }

    private static string GetClientIp(HttpContext context)
    {
        var ip = GetHeaderValue(context, "X-Forwarded-For", null);

        if (!string.IsNullOrEmpty(ip))
        {
            return ip;
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private static string GetHttpMethod(HttpContext context) =>
        context.Request.Method;

    private static string GetPath(HttpContext context) =>
        context.Request.Path;

    private static int GetStatusCode(HttpContext context) =>
        context.Response?.StatusCode ?? 0;

    private static async Task<string> ReadBodyAsync(Stream body)
    {
        body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(body, Encoding.UTF8, leaveOpen: true);
        var text = await reader.ReadToEndAsync();
        return text;
    }

    private static CompositeDisposable PushLogContextProperties((string Name, object Value)[] properties)
    {
        var disposables = properties
            .Select(property => LogContext.PushProperty(property.Name, property.Value))
            .ToList();

        return new CompositeDisposable(disposables);
    }

    private class CompositeDisposable(IEnumerable<IDisposable> disposables) : IDisposable
    {
        public void Dispose()
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }
    }
}

