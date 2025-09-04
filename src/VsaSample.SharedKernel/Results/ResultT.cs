namespace VsaSample.SharedKernel.Results;

public class Result<T>(
    T value,
    bool isSuccess,
    Error error)
    : Result(isSuccess, error)
{
    public T Value { get; set; } = value;

    public static Result<T> Success(T value) => new(value, true, Error.None);

    public new static Result<T> Failure(
        Error error) => new(default!, false, error);
    
    public new static Result<T> ValidationFailure(
        ValidationError validationError) => new(default!, false ,validationError);
}
