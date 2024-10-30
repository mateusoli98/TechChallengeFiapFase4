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

            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<DataContext>();

            builder.Services.AddSingleton<IContactRepository, ContactRepository>();
            builder.Services.AddSingleton<ICreateContactProcessingUseCase, CreateContactProcessingUseCase>();
            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}