namespace VsaSample.Api.Endpoints.Abstractions;

public abstract class EndpointGroupBase
{
    private string Section => GetType().Name;

    public abstract void Map(IEndpointRouteBuilder app);

    protected RouteGroupBuilder MapGroup(IEndpointRouteBuilder app)
    {
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(ApiEndpoints.V1))
            .HasApiVersion(new ApiVersion(ApiEndpoints.V2)) 
            .ReportApiVersions()
            .Build();

        var builder = app.MapGroup($"/api/v{{apiVersion:apiVersion}}/{Section.ToLowerInvariant()}")
            .WithTags($"{Section}")
            .WithOpenApi()
            .WithApiVersionSet(apiVersionSet);

        return builder;
    }
}

