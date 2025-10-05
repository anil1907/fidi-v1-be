namespace VsaSample.Application.Features.Templates.Shared;

public sealed record TemplateResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public IReadOnlyList<TemplateSectionResponse> Sections { get; init; } = Array.Empty<TemplateSectionResponse>();
    public bool IsActive { get; init; }
    public DateTime? CreateDate { get; init; }
}

public sealed record TemplateSectionResponse
(
    string Id,
    string Title,
    IReadOnlyList<TemplateSectionItemResponse> Items);

public sealed record TemplateSectionItemResponse
(
    string Id,
    string Label,
    string Amount,
    string? Note,
    int? Calories);
