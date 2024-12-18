using Application.UseCases.CreateContact;
using Application.UseCases.CreateContact.Interfaces;
using Domain.Repositories.Relational;
using Infra.Migrations;
using Infra.Persistence.Sql.Context;
using Infra.Persistence.Sql.Repositories;
using Infra.Services.Messages;
using Microsoft.EntityFrameworkCore;

namespace CreateWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Erro: DB_CONNECTIONSTRING n�o foi configurada.");
            }
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<DataContext>();

            builder.Services.AddSingleton<IContactRepository, ContactRepository>();
            builder.Services.AddSingleton<ICreateContactProcessingUseCase, CreateContactProcessingUseCase>();
            builder.Services.AddSingleton<IRabbitMqProducerService, RabbitMqProducerService>();
            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            MigrationHelper.ApplyMigrations<DataContext>(host);
            host.Run();
        }
    }
}