using VsaSample.Application.Features.DietPlans.Shared;

namespace VsaSample.Application.Features.DietPlans.Update;

public sealed record UpdateDietPlanCommand(
    Guid Id,
    Guid ClientId,
    Guid TemplateId,
    string Name,
    DateOnly DateStart,
    DateOnly DateEnd,
    string? Notes,
    IReadOnlyList<DietPlanSectionDto> Sections,
    bool? IsActive) : ICommand
{
    public string Culture { get; init; } = Cultures.Default;
}

public sealed record UpdateDietPlanRequest(
    Guid ClientId,
    Guid TemplateId,
    string Name,
    DateOnly DateStart,
    DateOnly DateEnd,
    string? Notes,
    IReadOnlyList<DietPlanSectionDto> Sections,
    bool? IsActive);
