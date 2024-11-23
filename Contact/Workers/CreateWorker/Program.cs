using Application.UseCases.CreateContact;
using Application.UseCases.CreateContact.Interfaces;
using Domain.Repositories.Relational;
using Infra.Persistence.Sql.Context;
using Infra.Persistence.Sql.Repositories;
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
                Console.WriteLine("Erro: DB_CONNECTIONSTRING não foi configurada.");
            }
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<DataContext>();

            builder.Services.AddSingleton<IContactRepository, ContactRepository>();
            builder.Services.AddSingleton<ICreateContactProcessingUseCase, CreateContactProcessingUseCase>();
            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            ApplyMigrations(host);
            host.Run();
        }

        private static void ApplyMigrations(IHost host)
        {
            using (var scope = host.Services.CreateScope())
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
    }
}