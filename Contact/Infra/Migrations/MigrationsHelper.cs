using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infra.Migrations;

public static class MigrationHelper
{
    public static void ApplyMigrations<TContext>(IHost host) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<TContext>();
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