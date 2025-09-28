using System.ComponentModel.DataAnnotations.Schema;

namespace VsaSample.Domain.Entities;

public sealed class Client : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string FirstName { get; set; } = string.Empty;

    [Sieve(CanFilter = true, CanSort = true)]
    public string LastName { get; set; } = string.Empty;

    [Sieve(CanFilter = true, CanSort = true)]
    public string Email { get; set; } = string.Empty;

    [Sieve(CanFilter = true, CanSort = true)]
    public string Phone { get; set; } = string.Empty;

    [NotMapped]
    public string FullName => string.Join(' ', new[] { FirstName, LastName }.Where(x => !string.IsNullOrWhiteSpace(x))).Trim();

    public string? Notes { get; set; }

    public List<string> Goals { get; private set; } = [];

    public void SetGoals(IEnumerable<string>? goals)
    {
        Goals = goals?.Where(goal => !string.IsNullOrWhiteSpace(goal))
            .Select(goal => goal.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList() ?? [];
    }
}
