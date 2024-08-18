using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Catalog.Infrastructure.Services;
using Catalog.Core.Interfaces;
using Carting.Infrastructure.Data;
using Common.RabbitMq;

namespace Catalog.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ConfigurationManager config)
    {
        services.Configure<RabbitMqConfiguration>(config.GetRequiredSection("RabbitMqOptions"));
        services.AddSingleton<RabbitMqClient>();
        services.AddLogging();

        var connectionString = config.GetConnectionString("SqliteConnection");
        Guard.Against.Null(connectionString);
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped<IDatabaseInitializationService, DatabaseInitializationService>();

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IItemService, ItemService>();

        return services;
    }
}