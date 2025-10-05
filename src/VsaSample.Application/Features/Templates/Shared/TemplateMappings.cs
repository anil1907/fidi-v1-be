namespace VsaSample.Application.Features.Templates.Shared;

public static class TemplateMappings
{
    public static TemplateResponse ToResponse(this Template template) => new()
    {
        Id = template.Id,
        Name = template.Name,
        Description = template.Description,
        Sections = template.Sections
            .Select(s => new TemplateSectionResponse(
                s.Id,
                s.Title,
                s.Items
                    .Select(i => new TemplateSectionItemResponse(i.Id, i.Label, i.Amount, i.Note, i.Calories))
                    .ToList()))
            .ToList(),
        IsActive = template.IsActive,
        CreateDate = template.CreateDate
    };
}
