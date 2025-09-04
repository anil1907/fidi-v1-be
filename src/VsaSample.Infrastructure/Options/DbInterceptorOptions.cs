namespace VsaSample.Infrastructure.Options;

public class DbInterceptorOptions
{
    public int SlowQueryThresholdMs { get; set; }
    public int SlowCommandThresholdMs { get; set; }

}