using Application.UseCases.DeleteContact;
using Application.UseCases.DeleteContact.Interfaces;
using Application.UseCases.DeleteContactPermanently;
using Application.UseCases.DeleteContactPermanently.Interfaces;
using Application.UseCases.GetContact;
using Domain.Repositories.Relational;
using Infra.Persistence.Sql.Context;
using Infra.Persistence.Sql.Repositories;
using Infra.Services.Messages;
using Microsoft.EntityFrameworkCore;

namespace DeleteAPI;

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

        app.MapControllers();

        return app;
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ISendDeleteContactRequestUseCase, SendDeleteContactRequestUseCase>();
        services.AddScoped<ISendDeleteContactPermanentlyRequestUseCase, SendDeleteContactPermanentlyRequestUseCase>();
        services.AddScoped<IGetContactUseCase, GetContactUseCase>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IContactRepository, ContactRepository>();

        return services;
    }

    public static IServiceCollection AddRabbitMQService(this IServiceCollection services, IConfiguration configuration)
    {

        //var server = configuration.GetSection("MassTransit")["Server"] ?? string.Empty;
        //var user = configuration.GetSection("MassTransit")["User"] ?? string.Empty;
        //var password = configuration.GetSection("MassTransit")["Password"] ?? string.Empty;

        //services.AddMassTransit(busConfigurator =>
        //{
        //    busConfigurator.UsingRabbitMq((ctx, cfg) =>
        //    {
        //        cfg.Host(new Uri(server), host =>
        //        {
        //            host.Username(user);
        //            host.Password(password);
        //        });

        //        cfg.ConfigureEndpoints(ctx);
        //    });
        //});

        services.AddScoped<IRabbitMqProducerService, RabbitMqProducerService>();

        return services;
    }
}
