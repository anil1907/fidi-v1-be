using Microsoft.AspNetCore.Http.Metadata;
using VsaSample.SharedKernel.Metadata;

namespace VsaSample.Api.Middlewares;

public class RequestValidationMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint is not null)
        {
            var expectsJson = endpoint.Metadata
                .OfType<IAcceptsMetadata>()
                .Any(m => m.ContentTypes.Contains(ContentTypes.ApplicationJson));

            if (expectsJson && context.Request.ContentLength == 0)
            {
                var error = new ValidationError([
                    Error.Problem("Validation.InvalidJson", CommonErrors.InvalidJson())
                ]);

                var result = Result.ValidationFailure(error);
                await result.ToHttpResponse().ExecuteAsync(context);
                return;
            }

            var expectsRequest = endpoint.Metadata.OfType<IAcceptsRequestMetadata>().Any();

            if (expectsRequest && context.Request.Query.Count == 0)
            {
                var error = new ValidationError([
                    Error.Problem("Validation.InvalidQuery", CommonErrors.InvalidQuery())
                ]);

                var result = Result.ValidationFailure(error);
                await result.ToHttpResponse().ExecuteAsync(context);
                return;
            }
        }

        await next(context);
    }
}

