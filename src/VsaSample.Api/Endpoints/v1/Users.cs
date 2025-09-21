namespace VsaSample.Api.Endpoints.v1;

internal sealed class Users : EndpointGroupBase
{
    public override void Map(IEndpointRouteBuilder app)
    {
        var route = MapGroup(app)
            .MapToApiVersion(ApiEndpoints.V1);

        route.MapPost("/login",
                async (LoginUserCommand command, ICommandHandler<LoginUserCommand, LoginUserResponse> handler,
                    CancellationToken ct) =>
                {
                    var result = await handler.Handle(command, ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Users.Login, ApiEndpoints.V1))
            .Produces<LoginUserResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .AcceptsJson<LoginUserCommand>()
            .AllowAnonymous();

        route.MapPost("/register",
                async (RegisterUserCommand command, ICommandHandler<RegisterUserCommand, Guid> handler,
                    CancellationToken ct) =>
                {
                    var result = await handler.Handle(command, ct);
                    return result.ToHttpResponse();
                })
            .WithName(ApiEndpoints.WithVersion(ApiEndpoints.Users.Register, ApiEndpoints.V1))
            .Produces<Guid>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .AcceptsJson<RegisterUserCommand>()
            .AllowAnonymous();
    }
}
