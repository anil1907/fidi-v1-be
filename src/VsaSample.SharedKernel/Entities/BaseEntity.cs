using System.ComponentModel.DataAnnotations;
using Sieve.Attributes;

namespace VsaSample.SharedKernel.Entities;

public abstract class BaseEntity
{
    [Required]
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public long Id { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime? CreateDate { get; set; } = DateTime.UtcNow;
    public long? CreatedBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public long? UpdatedBy { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public bool IsActive { get; set; }
}

