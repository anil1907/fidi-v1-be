namespace VsaSample.SharedKernel.Extensions;

public static class DateTimeExtensions
{
    public static DateTime EnsureUtc(this DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }

    public static DateTime? EnsureUtc(this DateTime? value)
    {
        return value.HasValue ? value.Value.EnsureUtc() : null;
    }
}
