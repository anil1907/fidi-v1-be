namespace VsaSample.SharedKernel.Caching;

public class TagTemplateKeys(string slice)
{
    public string ById { get; } = $"{slice}:{{Id}}";
    public string List { get; } = $"{slice}:List";

    public string Custom(string name) => $"{slice}:{name}";
}
