namespace VsaSample.Application.Features.DietPlans.Shared;

public sealed record DietPlanSectionDto(
    string? Id,
    string Title,
    IReadOnlyList<DietPlanSectionItemDto>? Items);

public sealed record DietPlanSectionItemDto(
    string? Id,
    string Label,
    string Amount,
    string? Note,
    int? Calories);
