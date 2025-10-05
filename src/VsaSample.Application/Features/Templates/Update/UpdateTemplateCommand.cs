namespace VsaSample.Application.Features.Templates.Update;

using VsaSample.Application.Features.Templates.Create;

public sealed record UpdateTemplateCommand(
    Guid Id,
    string Name,
    string? Description,
    IReadOnlyCollection<TemplateSectionDto> Sections,
    bool? IsActive) : ICommand
{
    public string Culture { get; init; } = Cultures.Default;
}

public sealed record UpdateTemplateRequest(
    string Name,
    string? Description,
    IReadOnlyCollection<TemplateSectionDto> Sections,
    bool? IsActive);
