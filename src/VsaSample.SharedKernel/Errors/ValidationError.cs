using VsaSample.SharedKernel.Results;

namespace VsaSample.SharedKernel.Errors;

public sealed record ValidationError : Error
{
    public Error[] Errors { get; init; }

    public ValidationError(Error[] errors)
        : base(
            "Validation.General",
            ValidationErrors.GeneralEn,
            ErrorType.Validation)
    {
        Errors = errors;

        WithDescription(Cultures.Tr, ValidationErrors.GeneralTr);
        WithDescription(Cultures.En, ValidationErrors.GeneralEn);
    }

    public static ValidationError FromResults(IEnumerable<Result> results) =>
        new(results.Where(r => r.IsFailure).Select(r => r.Error).ToArray());
}

