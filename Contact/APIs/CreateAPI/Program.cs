using Infra.Migrations;
using Infra.Persistence.Sql.Context;
using OpenTelemetry.Metrics;
using Prometheus;


namespace CreateAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOpenTelemetry().WithMetrics(builder =>
                {
                    builder.AddPrometheusExporter();
                    builder.AddAspNetCoreInstrumentation();
                    builder.AddRuntimeInstrumentation();
                    builder.AddProcessInstrumentation();
                    builder.AddMeter("Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel");
                    builder.AddView("http.server.request.duration",
                        new ExplicitBucketHistogramConfiguration
                        {
                            Boundaries = new double[]
                            {
                                0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1,
                                0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10
                            }
                        });

                });            

            builder.ConfigureServices();            

            var app = builder.Build();

            app.MapPrometheusScrapingEndpoint();

            app.ConfigureApp();

            var counter = Metrics.CreateCounter("createapi", "Counts requests API endpoints",
                new CounterConfiguration
                {
                    LabelNames = ["method", "endpoint"]
                });

            app.Use((context, next) =>
            {
                counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                return next();
            });

            app.UseHttpMetrics();
            app.UseMetricServer();
            MigrationHelper.ApplyMigrations<DataContext>(app);

            app.Run();


        }

    }

}
