namespace VsaSample.Application.Features.Templates.Create;

using VsaSample.Application.Features.Templates.Shared;

public sealed record CreateTemplateCommand(
    string Name,
    string? Description,
    IReadOnlyCollection<TemplateSectionDto> Sections) : ICommand<Guid>
{
    public string Culture { get; init; } = Cultures.Default;
}

public sealed record TemplateSectionDto(
    string Id,
    string Title,
    IReadOnlyCollection<TemplateSectionItemDto> Items);

public sealed record TemplateSectionItemDto(
    string Id,
    string Label,
    string Amount,
    string? Note,
    int? Calories);
