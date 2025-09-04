using System.Data.Common;
using Microsoft.Extensions.Logging;

namespace VsaSample.Infrastructure.Database.Interceptors;

internal class SlowCommandInterceptor(
    ILogger<SlowCommandInterceptor> logger,
    IOptions<DbInterceptorOptions> opts)
    : DbCommandInterceptor
{
    private readonly DbInterceptorOptions _opts = opts.Value;
    
    public override ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        var elapsed = eventData.Duration.TotalMilliseconds;
        if (elapsed > _opts.SlowQueryThresholdMs)
        {
            logger.LogWarning(
                "Slow query detected ({Elapsed} ms): {CommandText}",
                elapsed,
                command.CommandText);
        }

        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override ValueTask<int> NonQueryExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        var elapsed = eventData.Duration.TotalMilliseconds;
        if (elapsed > _opts.SlowCommandThresholdMs)
        {
            logger.LogWarning(
                "Slow command detected ({Elapsed} ms): {CommandText}",
                elapsed,
                command.CommandText);
        }

        return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }
}
