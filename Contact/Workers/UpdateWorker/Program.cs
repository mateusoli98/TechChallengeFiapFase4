using Application.UseCases.UpdateContact;
using Application.UseCases.UpdateContact.Interfaces;
using Domain.Repositories.Relational;
using Infra.Persistence.Sql.Context;
using Infra.Persistence.Sql.Repositories;
using Microsoft.EntityFrameworkCore;


namespace UpdateWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<DataContext>();

            builder.Services.AddSingleton<IContactRepository, ContactRepository>();
            builder.Services.AddSingleton<IUpdateContactProcessingUseCase, UpdateContactProcessingUseCase>();
            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}