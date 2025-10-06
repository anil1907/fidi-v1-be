using VsaSample.Application.Features.DietPlans.Shared;

namespace VsaSample.Application.Features.DietPlans.Create;

public sealed record CreateDietPlanCommand(
    Guid ClientId,
    Guid TemplateId,
    string Name,
    DateOnly DateStart,
    DateOnly DateEnd,
    string? Notes,
    IReadOnlyList<DietPlanSectionDto> Sections) : ICommand<Guid>
{
    public string Culture { get; init; } = Cultures.Default;
}
