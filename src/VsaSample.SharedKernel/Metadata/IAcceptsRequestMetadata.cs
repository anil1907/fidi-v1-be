namespace VsaSample.SharedKernel.Metadata;

public interface IAcceptsRequestMetadata
{
    Type RequestType { get; }
}

