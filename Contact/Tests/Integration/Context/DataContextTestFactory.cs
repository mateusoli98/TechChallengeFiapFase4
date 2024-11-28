using Infra.Persistence.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Integration.Context;

public class DataContextTestFactory
{
    public static DataContext CreateInMemoryDataContext()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "IntegrationTestDatabase")
            .UseInternalServiceProvider(serviceProvider)
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new DataContext(options);
    }

    public static IServiceScopeFactory CreateInMemoryServiceScopeFactory()
    {
        var dataContext = CreateInMemoryDataContext();
        var serviceScope = new Mock<IServiceScope>();
        serviceScope.Setup(x => x.ServiceProvider.GetService(typeof(DataContext))).Returns(dataContext);

        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);

        return serviceScopeFactory.Object;
    }
}
