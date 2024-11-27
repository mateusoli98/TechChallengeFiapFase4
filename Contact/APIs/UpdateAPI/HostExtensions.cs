using Application.UseCases.GetContact;
using Application.UseCases.UpdateContact;
using Application.UseCases.UpdateContact.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories.Relational;
using Infra.Persistence.Sql.Repositories;
using Infra.Persistence.Sql.Context;
using Infra.Services.Messages;
using Infra.Extensions;

namespace UpdateAPI;

public static class HostExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddRabbitMQService(builder.Configuration);
        builder.Services.AddRepositories(builder.Configuration);
        builder.Services.AddUseCases();

        builder.Services.AddRabbitMQHealthChecks();
        builder.Services.AddSQLHealthChecks();
        builder.Services.AddCustomOpenTelemetry();

        return builder;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        app.MapControllers();
        app.MapHealthChecks("/health");
        app.MapCustomHealthChecksEndpoints();
        app.ApplyMigrations();

        return app;
    }

    private static void ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                context.Database.Migrate(); // Aplica as migrações pendentes
                Console.WriteLine("Migrações aplicadas com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao aplicar migrações: {ex.Message}");
            }
        }
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ISendUpdateContactRequestUseCase, SendUpdateContactRequestUseCase>();
        services.AddScoped<IGetContactUseCase, GetContactUseCase>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING"));
        });

        services.AddScoped<IContactRepository, ContactRepository>();

        return services;
    }

    public static IServiceCollection AddRabbitMQService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRabbitMqProducerService, RabbitMqProducerService>();

        return services;
    }
}
