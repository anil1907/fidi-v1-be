namespace VsaSample.SharedKernel.Results;

public class PagedResult<T> : Result
{
    private PagedResult(
        IReadOnlyList<T>? items,
        int pageNumber,
        int pageSize,
        int totalCount,
        bool isSuccess,
        Error error)
        : base(isSuccess, error)
    {
        Items = items ?? [];
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = pageSize > 0
            ? (int)Math.Ceiling(totalCount / (double)pageSize)
            : 0;
    }

    public IReadOnlyList<T> Items { get; }
    private int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    private int TotalPages { get; }
    
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    private static PagedResult<T> Success(
        IEnumerable<T> items,
        int pageNumber,
        int pageSize,
        int totalCount)
        => new(items.ToList(), pageNumber, pageSize, totalCount,
               true, Error.None);

    public static PagedResult<T> Create(
        IEnumerable<T> items,
        int pageNumber,
        int pageSize,
        int totalCount)
        => Success(items, pageNumber, pageSize, totalCount);

    public new static PagedResult<T> Failure(
        Error error)
        => new(null, 0, 0, 0, false, error);

    public new static PagedResult<T> ValidationFailure(ValidationError validationError)
        => new(null, 0, 0, 0, false, validationError);
}
