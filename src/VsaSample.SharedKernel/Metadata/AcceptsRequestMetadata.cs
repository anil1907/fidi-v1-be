namespace VsaSample.SharedKernel.Metadata;

public sealed class AcceptsRequestMetadata<T> : IAcceptsRequestMetadata
{
    public Type RequestType => typeof(T);
}

