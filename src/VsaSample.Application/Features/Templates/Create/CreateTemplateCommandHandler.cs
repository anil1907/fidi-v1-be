namespace VsaSample.Application.Features.Templates.Create;

using VsaSample.Application.Features.Templates.Shared;

public sealed class CreateTemplateCommandHandler(
    ITemplateRepository repository,
    ILogger<CreateTemplateCommandHandler> logger)
    : ICommandHandler<CreateTemplateCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var normalizedName = request.Name.Trim().ToLowerInvariant();
        var nameExists = await repository.Query
            .AnyAsync(template => template.Name.ToLower() == normalizedName, cancellationToken);
        if (nameExists)
        {
            return Result<Guid>.Failure(TemplateConstants.Errors.NameNotUnique(request.Name).Localize(request.Culture));
        }

        var template = new Template(request.Name, request.Description);
        template.SetSections(request.Sections.Select(section => new TemplateSection(
            section.Id,
            section.Title,
            section.Items?.Select(item => new TemplateSectionItem(item.Id, item.Label, item.Amount, item.Note, item.Calories)).ToList() ?? new List<TemplateSectionItem>()
        )));
        template.IsActive = true;

        var id = await repository.AddAsync(template, cancellationToken);

        logger.LogInformation("Template {TemplateId} created", id);

        return Result<Guid>.Success(id);
    }
}


