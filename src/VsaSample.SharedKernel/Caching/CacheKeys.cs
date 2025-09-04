namespace VsaSample.SharedKernel.Caching;

public class CacheKeys(string slice)
{
    public string PagePrefix { get; } = $"{slice}Page";
    public string ListTag { get; } = $"{slice}:List";

    public string Custom(string name) => $"{slice}:{name}";
}
