using FluentValidation.Results;

namespace VsaSample.Application.Behaviours.Validation;

public static class ValidationDecorator
{
    // For: ICommand<TResponse>
    public sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken ct)
        {
            var failures = await ValidateAsync(command, validators, ct);

            if (failures.Length == 0)
                return await innerHandler.Handle(command, ct);

            return Result<TResponse>.ValidationFailure(CreateValidationError(failures));
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken ct)
        {
            var failures = await ValidateAsync(command, validators, ct);

            if (failures.Length == 0)
                return await innerHandler.Handle(command, ct);

            return Result.ValidationFailure(CreateValidationError(failures));
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        IEnumerable<IValidator<TQuery>> validators)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken ct)
        {
            var failures = await ValidateAsync(query, validators, ct);

            if (failures.Length == 0)
                return await innerHandler.Handle(query, ct);

            return Result<TResponse>.ValidationFailure(CreateValidationError(failures));
        }
    }

    private static async Task<ValidationFailure[]> ValidateAsync<T>(
        T model,
        IEnumerable<IValidator<T>> validators,
        CancellationToken ct)
    {
        var enumerable = validators as IValidator<T>[] ?? validators.ToArray();
        if (!enumerable.Any())
            return [];

        var ctx = new ValidationContext<T>(model);

        var results = await Task.WhenAll(enumerable.Select(v => v.ValidateAsync(ctx, ct)));
        return results
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToArray()!;
    }

    private static ValidationError CreateValidationError(ValidationFailure[] failures)
    {
        var errors = failures
            .GroupBy(f => f.PropertyName ?? string.Empty, StringComparer.Ordinal)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).Where(m => !string.IsNullOrWhiteSpace(m)).Distinct().ToArray(),
                StringComparer.Ordinal);

        var errorCodes = failures
            .GroupBy(f => f.PropertyName ?? string.Empty, StringComparer.Ordinal)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToArray(),
                StringComparer.Ordinal);

        var flat = failures.Select(f => Error.Problem(f.ErrorCode, f.ErrorMessage)).ToArray();

        return new ValidationError(flat);
    }
}
