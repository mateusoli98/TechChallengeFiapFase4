using Microsoft.EntityFrameworkCore;
using Application.UseCases.CreateContact.Interfaces;
using Application.UseCases.CreateContact;
using Domain.Repositories.Relational;
using Infra.Persistence.Sql.Repositories;
using Infra.Persistence.Sql.Context;
using Infra.Services.Messages;
using Infra.Extensions;


namespace CreateAPI;

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

        return app;
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ISendCreateContactRequestUseCase, SendCreateContactRequestUseCase>();

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

    public static IServiceCollection AddRabbitMQService( this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRabbitMqProducerService, RabbitMqProducerService>();

        return services;
    }
}
