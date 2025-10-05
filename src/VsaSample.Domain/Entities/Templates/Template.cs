using System.Text.Json.Serialization;

namespace VsaSample.Domain.Entities.Templates;

public sealed class Template : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; private set; } = string.Empty;

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Description { get; private set; }

    public List<TemplateSection> Sections { get; private set; } = [];

    private Template()
    {
    }

    public Template(string name, string? description)
    {
        SetName(name);
        SetDescription(description);
    }

    public void SetName(string name)
    {
        Name = string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("Name cannot be empty", nameof(name))
            : name.Trim();
    }

    public void SetDescription(string? description) => Description = string.IsNullOrWhiteSpace(description)
        ? null
        : description.Trim();

    public void SetSections(IEnumerable<TemplateSection> sections)
    {
        Sections = sections?.ToList() ?? [];
    }
}

public sealed class TemplateSection
{
    public string Id { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public List<TemplateSectionItem> Items { get; private set; } = [];

    private TemplateSection()
    {
    }

    [JsonConstructor]
    public TemplateSection(string id, string title, List<TemplateSectionItem>? items)
    {
        Id = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id;
        Title = string.IsNullOrWhiteSpace(title)
            ? throw new ArgumentException("Section title cannot be empty", nameof(title))
            : title.Trim();
        SetItems(items);
    }

    public void SetItems(IEnumerable<TemplateSectionItem>? items)
    {
        Items = items?.ToList() ?? [];
    }
}

public sealed class TemplateSectionItem
{
    public string Id { get; private set; } = string.Empty;
    public string Label { get; private set; } = string.Empty;
    public string Amount { get; private set; } = string.Empty;
    public string? Note { get; private set; }
    public int? Calories { get; private set; }

    private TemplateSectionItem()
    {
    }

    [JsonConstructor]
    public TemplateSectionItem(string id, string label, string amount, string? note, int? calories)
    {
        Id = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id;
        Label = string.IsNullOrWhiteSpace(label)
            ? throw new ArgumentException("Item label cannot be empty", nameof(label))
            : label.Trim();
        Amount = string.IsNullOrWhiteSpace(amount) ? string.Empty : amount.Trim();
        Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
        Calories = calories;
    }
}

