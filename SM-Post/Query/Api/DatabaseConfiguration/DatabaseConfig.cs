using InfrastructureLayer.DataAccess;

namespace Api.DatabaseConfiguration;

public static class DatabaseConfig
{
    public static void SetupDatabase(this IServiceCollection services)
    {
        var dataContext = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        dataContext.Database.EnsureCreated();
    }
}

