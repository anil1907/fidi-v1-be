namespace VsaSample.SharedKernel.Entities;

public abstract class TranslationBaseEntity : BaseEntity
{
    public string Culture { get; set; } = null!;
}
