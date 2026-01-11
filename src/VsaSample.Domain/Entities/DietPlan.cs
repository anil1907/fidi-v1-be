using VsaSample.Domain.Entities.Templates;

namespace VsaSample.Domain.Entities;

public sealed class DietPlan : BaseEntity, IOrganizationScoped
{
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;

    [Sieve(CanFilter = true, CanSort = true)]
    public Guid ClientId { get; set; }

    public Client Client { get; set; } = null!;

    [Sieve(CanFilter = true, CanSort = true)]
    public Guid TemplateId { get; set; }

    public Template Template { get; set; } = null!;

    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; } = string.Empty;

    [Sieve(CanFilter = true, CanSort = true)]
    public DateOnly DateStart { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateOnly DateEnd { get; set; }

    public string? Notes { get; set; }

    public List<TemplateSection> Sections { get; private set; } = [];

    public void SetSections(IEnumerable<TemplateSection> sections)
    {
        Sections = sections?.ToList() ?? [];
    }
}
