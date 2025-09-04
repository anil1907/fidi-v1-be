namespace VsaSample.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        var problemDetails = CreateProblemDetails(result);

        return Results.Problem(problemDetails);
    }

    public static IResult ToHttpResponse(this Result result)
    {
        if (result.IsSuccess)
            return Results.Ok();

        var problemDetails = CreateProblemDetails(result);

        return Results.Problem(problemDetails);
    }

    private static Microsoft.AspNetCore.Mvc.ProblemDetails CreateProblemDetails(Result result)
    {
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = result.Error.Type switch
            {
                ErrorType.Validation or ErrorType.Problem => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.NotFound   => StatusCodes.Status404NotFound,
                _                    => StatusCodes.Status500InternalServerError
            },
            Type = result.Error.Type switch
            {
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            },
            Title = result.Error.Type.ToString(),
            Extensions = GetErrors(result)
        };
        return problemDetails;
    }
    
    static Dictionary<string, object?> GetErrors(Result result)
    {
        var errors = result.Error is ValidationError validationError
            ? validationError.Errors.Select(e => e.ToErrorResponse())
            : new[] { result.Error.ToErrorResponse() };

        return new Dictionary<string, object?>
        {
            { "errors", errors }
        };
    }
}
