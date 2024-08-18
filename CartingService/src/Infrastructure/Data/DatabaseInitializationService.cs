using Carting.Core.CartAggregate;
using Carting.Core.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Carting.Infrastructure.Data;

public class DatabaseInitializationService(
    ICartRepository _repository,
    ILogger<DatabaseInitializationService> _logger)
    : IDatabaseInitializationService
{
    public async Task Initalize()
    {
        try
        {
            var carts = await _repository.GetAllAsync();

            if (carts is null || !carts.Any())
            {
                await SeedCarts();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured seeding the DB. {exceptionMessage}", ex.Message);
        }
    }

    private async Task SeedCarts()
    {
        var cart = new Cart
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Items =
            [
                new Item
                {
                    Id = 1,
                    Name = "Item 1",
                    Price = 1,
                    Quantity = 1
                },
                new Item
                {
                    Id = 2,
                    Name = "Item 2",
                    Price = 1,
                    Quantity = 1
                },
            ]
        };

        await _repository.AddCartAsync(cart);
    }
}
