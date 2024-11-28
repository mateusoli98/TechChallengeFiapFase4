using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;

namespace Infra.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddCustomOpenTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddPrometheusExporter();
            });

        return services;
    }
}
