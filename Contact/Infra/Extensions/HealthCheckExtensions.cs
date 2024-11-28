using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace Infra.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddRabbitMQHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddRabbitMQ(
                rabbitConnectionString: $"amqp://{Environment.GetEnvironmentVariable("RABBITMQ_USER")}:{Environment.GetEnvironmentVariable("RABBITMQ_PASS")}@{Environment.GetEnvironmentVariable("RABBITMQ_HOST")}",
                name: "RabbitMQ",
                tags: ["readiness"]
            );

        return services;
    }

    public static IServiceCollection AddSQLHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddSqlServer(
                connectionString: Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING") ?? string.Empty,
                name: "SQL Server",
                tags: ["database", "readiness"]
            );

        return services;
    }

    public static void MapCustomHealthChecksEndpoints(this WebApplication app)
    {
        // Mapear Health Checks para readiness e liveness
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("readiness"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false, // Apenas verifica se o app está respondendo
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Endpoint para Prometheus
        app.UseMetricServer("/prometheus-metrics");
    }
}
