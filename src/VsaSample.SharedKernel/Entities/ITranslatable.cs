namespace VsaSample.SharedKernel.Entities;

public interface ITranslatable<TTranslation>
{
    List<TTranslation> Translations { get; }
}
