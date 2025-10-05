namespace VsaSample.Application.Features.Templates.GetById;

using VsaSample.Application.Features.Templates.Shared;

internal sealed class GetTemplateByIdQueryHandler(
    ITemplateRepository repository,
    ILogger<GetTemplateByIdQueryHandler> logger)
    : IQueryHandler<GetTemplateByIdQuery, TemplateResponse>
{
    public async Task<Result<TemplateResponse>> Handle(GetTemplateByIdQuery query, CancellationToken cancellationToken)
    {
        var template = await repository.GetByIdAsync(query.Id, cancellationToken);
        if (template is null)
        {
            return Result<TemplateResponse>.Failure(TemplateConstants.Errors.NotFound(query.Id));
        }

        logger.LogInformation("Fetched template {TemplateId}", query.Id);
        return Result<TemplateResponse>.Success(template.ToResponse());
    }
}
