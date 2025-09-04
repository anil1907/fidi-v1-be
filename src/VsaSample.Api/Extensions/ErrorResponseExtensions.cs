namespace VsaSample.Api.Extensions;

using VsaSample.Api.Contracts;
using VsaSample.SharedKernel.Errors;

public static class ErrorResponseExtensions
{
    public static ErrorResponse ToErrorResponse(this Error error) => new()
    {
        Code = error.Code,
        Description = error.Descriptions.Count > 0 ? null : error.Description,
        Type = error.Type,
        Descriptions = error.Descriptions.Count > 0 ? error.Descriptions : null
    };
}

