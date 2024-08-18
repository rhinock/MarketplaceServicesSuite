using Carting.Core.Interfaces;
using Carting.Infrastructure.Data;
using Carting.Infrastructure.Services;
using Common.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carting.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager config)
    {
        services.Configure<RabbitMqConfiguration>(config.GetRequiredSection("RabbitMqOptions"));
        services.AddSingleton<RabbitMqClient>();
        services.AddLogging();

        services.Configure<MongoDbConfiguration>(config.GetSection("MongoDbOptions"));
        services.AddSingleton<AppDbContext>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IDatabaseInitializationService, DatabaseInitializationService>();

        services.AddHostedService<ItemUpdateListenerService>();
        services.AddHostedService<ItemDeleteListenerService>();

        return services;
    }
}