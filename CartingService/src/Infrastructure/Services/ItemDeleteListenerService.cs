using Carting.Core.Interfaces;
using Common.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Carting.Infrastructure.Services;

internal class ItemDeleteListenerService
    (RabbitMqClient _rabbitMqClient,
    IServiceScopeFactory _serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _rabbitMqClient.Consume(ItemDeleteHandler);
        _rabbitMqClient.ConsumeDeadLetter(ItemDeleteHandler);

        await Task.CompletedTask;
    }

    private async void ItemDeleteHandler(ItemDeleteMessage message)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var cartRepository = scope.ServiceProvider.GetRequiredService<ICartRepository>();

        var carts = await cartRepository.GetAllByItemIdsAsync(message.Ids);

        foreach (var cart in carts)
        {
            await cartRepository.RemoveItemsAsync(cart.Id, message.Ids);
        }
    }
}