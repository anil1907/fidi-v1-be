namespace VsaSample.Api.Contracts;

public sealed class ErrorResponse
{
    public string Code { get; init; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    public ErrorType Type { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, string>? Descriptions { get; init; }
}

