using System.Text.Json;
using FluentValidation.Results;

namespace VsaSample.Api.Extensions;

public static class ProblemDetailExtensions
{
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static async Task WriteProblemAsync(HttpContext ctx, int status, string title, string detail, string? type = "about:blank")
    {
        var pd = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Type = type,
            Instance = ctx.Request.Path,
            Extensions =
            {
                ["traceId"] = ctx.TraceIdentifier
            }
        };

        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/problem+json";

        var svc = ctx.RequestServices.GetService<IProblemDetailsService>();
        if (svc is not null)
        {
            await svc.WriteAsync(new ProblemDetailsContext { HttpContext = ctx, ProblemDetails = pd });
            return;
        }

        await ctx.Response.WriteAsJsonAsync(pd, JsonOpts, ctx.RequestAborted);
    }

    public static async Task WriteValidationProblemAsync(HttpContext ctx, IEnumerable<ValidationFailure> failures)
    {
        var validationFailures = failures.ToList();
        var errors = validationFailures
            .GroupBy(f => f.PropertyName ?? string.Empty)
            .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).Distinct().ToArray());

        var vpd = new HttpValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Instance = ctx.Request.Path
        };

        // Optional: surface FV error codes
        var codes = validationFailures
            .GroupBy(f => f.PropertyName ?? string.Empty)
            .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToArray());

        if (codes.Values.Any(x => x.Length > 0))
            vpd.Extensions["errorCodes"] = codes;

        vpd.Extensions["traceId"] = ctx.TraceIdentifier;

        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        ctx.Response.ContentType = "application/problem+json";

        var svc = ctx.RequestServices.GetService<IProblemDetailsService>();
        if (svc is not null)
        {
            await svc.WriteAsync(new ProblemDetailsContext { HttpContext = ctx, ProblemDetails = vpd });
            return;
        }

        await ctx.Response.WriteAsJsonAsync(vpd, JsonOpts, ctx.RequestAborted);
    }
}