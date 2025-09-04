namespace VsaSample.SharedKernel.Errors;

public record Error(string Code, string Description, ErrorType Type)
{
    private readonly Dictionary<string, string> _localizedDescriptions = new();

    public IReadOnlyDictionary<string, string> Descriptions => _localizedDescriptions;

    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Problem(string code, string description) =>
        new(code, description, ErrorType.Problem);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    public Error WithDescription(string language, string description)
    {
        _localizedDescriptions[language] = description;
        return this;
    }

    public string GetDescription(string language) =>
        _localizedDescriptions.GetValueOrDefault(language, Description);

    public Error Localize(string language) =>
        new(Code, GetDescription(language), Type);
}