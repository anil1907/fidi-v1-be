namespace VsaSample.SharedKernel.Results;

public class Result
{
    protected Result(
        bool isSuccess,
        Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; set; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, Error.None);

    public static Result Failure(
        Error error) => new(false,  error);

    public static Result ValidationFailure(
        ValidationError validationError) => new(false, validationError);
    
    
}
