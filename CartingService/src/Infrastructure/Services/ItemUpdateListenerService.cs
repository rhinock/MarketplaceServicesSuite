using AutoMapper;
using Carting.Core.Interfaces;
using Common.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Carting.Infrastructure.Services;

internal class ItemUpdateListenerService(
    RabbitMqClient _rabbitMqClient,
    IServiceScopeFactory _serviceScopeFactory,
    IMapper _mapper) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _rabbitMqClient.Consume(ItemUpdateHandler);
        _rabbitMqClient.ConsumeDeadLetter(ItemUpdateHandler);

        await Task.CompletedTask;
    }

    private async void ItemUpdateHandler(ItemUpdateMessage message)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var cartRepository = scope.ServiceProvider.GetRequiredService<ICartRepository>();

        var carts = await cartRepository.GetAllByItemIdAsync(message.Id);

        foreach (var cart in carts)
        {
            var item = cart.Items.FirstOrDefault(x => x.Id == message.Id);

            if (item is not null)
            {
                _mapper.Map(message, item);
                await cartRepository.UpdateCartAsync(cart);
            }
        }
    }
}