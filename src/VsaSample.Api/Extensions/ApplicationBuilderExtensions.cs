namespace VsaSample.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
    {
        app.UseSwagger();

        var descriptions = app.DescribeApiVersions();

        app.UseSwaggerUI(options =>
        {
            foreach (var description in descriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"VsaSample.API {description.GroupName.ToUpperInvariant()}");
            }
        });

        return app;
    }
    
}