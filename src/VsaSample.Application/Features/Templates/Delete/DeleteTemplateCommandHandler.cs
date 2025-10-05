namespace VsaSample.Application.Features.Templates.Delete;

internal sealed class DeleteTemplateCommandHandler(
    ITemplateRepository repository,
    ILogger<DeleteTemplateCommandHandler> logger)
    : ICommandHandler<DeleteTemplateCommand>
{
    public async Task<Result> Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await repository.GetEntityByIdAsync(request.Id, cancellationToken);
        if (template is null)
        {
            return Result.Failure(TemplateConstants.Errors.NotFound(request.Id));
        }

        await repository.DeleteAsync(template, cancellationToken);

        logger.LogInformation("Template {TemplateId} deleted", request.Id);
        return Result.Success();
    }
}
