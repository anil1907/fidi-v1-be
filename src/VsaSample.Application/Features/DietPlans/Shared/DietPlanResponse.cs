namespace VsaSample.Application.Features.DietPlans.Shared;

public sealed record DietPlanResponse
{
    public Guid Id { get; init; }
    public Guid ClientId { get; init; }
    public Guid TemplateId { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateOnly DateStart { get; init; }
    public DateOnly DateEnd { get; init; }
    public string? Notes { get; init; }
    public IReadOnlyList<DietPlanSectionResponse> Sections { get; init; } = Array.Empty<DietPlanSectionResponse>();
    public bool IsActive { get; init; }
    public DateTime? CreateDate { get; init; }
}

public sealed record DietPlanSectionResponse(
    string Id,
    string Title,
    IReadOnlyList<DietPlanSectionItemResponse> Items);

public sealed record DietPlanSectionItemResponse(
    string Id,
    string Label,
    string Amount,
    string? Note,
    int? Calories);
