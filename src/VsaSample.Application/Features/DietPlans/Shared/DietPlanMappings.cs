namespace VsaSample.Application.Features.DietPlans.Shared;

public static class DietPlanMappings
{
    public static DietPlanResponse ToResponse(this DietPlan plan) => new()
    {
        Id = plan.Id,
        ClientId = plan.ClientId,
        TemplateId = plan.TemplateId,
        Name = plan.Name,
        DateStart = plan.DateStart,
        DateEnd = plan.DateEnd,
        Notes = plan.Notes,
        Sections = plan.Sections
            .Select(section => new DietPlanSectionResponse(
                section.Id,
                section.Title,
                section.Items
                    .Select(item => new DietPlanSectionItemResponse(item.Id, item.Label, item.Amount, item.Note, item.Calories))
                    .ToList()))
            .ToList(),
        IsActive = plan.IsActive,
        CreateDate = plan.CreateDate
    };
}
