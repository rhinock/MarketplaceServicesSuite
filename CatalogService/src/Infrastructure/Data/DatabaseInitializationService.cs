using Catalog.Core.Categories;
using Catalog.Core.Interfaces;
using Catalog.Core.Items;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Carting.Infrastructure.Data;

public class DatabaseInitializationService(
    IServiceProvider _services,
    ILogger<DatabaseInitializationService> _logger)
    : IDatabaseInitializationService
{
    public async Task Initalize()
    {
        using var scope = _services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            await context.Database.MigrateAsync();

            if (context.Categories.Any()) return;

            await PopulateTestData(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured seeding the DB. {exceptionMessage}", ex.Message);
        }
    }

    private static async Task PopulateTestData(AppDbContext dbContext)
    {
        var parentCategory = new Category
        {
            Id = 1,
            Name = "Category 1"
        };

        await dbContext.Categories.AddAsync(parentCategory);
        await dbContext.SaveChangesAsync();

        var childCategory = new Category
        {
            Id = 2,
            Name = "Category 2",
            ParentId = 1
        };

        dbContext.Categories.Add(childCategory);
        dbContext.SaveChanges();

        var parentItem = new Item
        {
            Id = 1,
            Name = "Item 1",
            CategoryId = 1,
            Price = 1,
            Amount = 1
        };

        var childItem = new Item
        {
            Id = 2,
            Name = "Item 2",
            CategoryId = 2,
            Price = 1,
            Amount = 1
        };

        await dbContext.Items.AddRangeAsync(new List<Item> { parentItem, childItem });
        await dbContext.SaveChangesAsync();
    }
}
