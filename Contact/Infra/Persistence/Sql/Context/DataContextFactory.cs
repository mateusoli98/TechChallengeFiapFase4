using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infra.Persistence.Sql.Context;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        //TODO: atualizar
        var basePath = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddUserSecrets<DataContextFactory>()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");

        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSqlServer(connectionString);

        return new DataContext(optionsBuilder.Options);
    }
}
