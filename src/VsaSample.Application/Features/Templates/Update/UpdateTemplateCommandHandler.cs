using VsaSample.Domain.Entities.Templates;

namespace VsaSample.Application.Features.Templates.Update;

using VsaSample.Application.Features.Templates.Create;

internal sealed class UpdateTemplateCommandHandler(
    ITemplateRepository repository,
    ILogger<UpdateTemplateCommandHandler> logger)
    : ICommandHandler<UpdateTemplateCommand>
{
    public async Task<Result> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await repository.GetEntityByIdAsync(request.Id, cancellationToken);
        if (template is null)
        {
            return Result.Failure(TemplateConstants.Errors.NotFound(request.Id).Localize(request.Culture));
        }

        var normalizedName = request.Name.Trim().ToLowerInvariant();
        var nameInUse = await repository.Query
            .AnyAsync(t => t.Id != request.Id && t.Name.ToLower() == normalizedName, cancellationToken);
        if (nameInUse)
        {
            return Result.Failure(TemplateConstants.Errors.NameNotUnique(request.Name).Localize(request.Culture));
        }

        template.SetName(request.Name);
        template.SetDescription(request.Description);
        template.SetSections(request.Sections.Select(ToSection));

        if (request.IsActive.HasValue)
        {
            template.IsActive = request.IsActive.Value;
        }

        await repository.UpdateAsync(template, cancellationToken);

        logger.LogInformation("Template {TemplateId} updated", template.Id);

        return Result.Success();
    }

    private static TemplateSection ToSection(TemplateSectionDto section) =>
        new(section.Id, section.Title, section.Items?.Select(ToItem).ToList() ?? new List<TemplateSectionItem>());

    private static TemplateSectionItem ToItem(TemplateSectionItemDto item) =>
        new(item.Id, item.Label, item.Amount, item.Note, item.Calories);
}

