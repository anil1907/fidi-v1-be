using Microsoft.AspNetCore.Builder;
using VsaSample.SharedKernel.Metadata;

namespace VsaSample.Infrastructure.Validation;

public static class EndpointValidationExtensions
{
    public static RouteHandlerBuilder AcceptsJson<T>(this RouteHandlerBuilder b) where T : notnull =>
        b.Accepts<T>(ContentTypes.ApplicationJson);

    public static RouteHandlerBuilder AcceptsRequest<T>(this RouteHandlerBuilder b) =>
        b.WithMetadata(new AcceptsRequestMetadata<T>());
}
