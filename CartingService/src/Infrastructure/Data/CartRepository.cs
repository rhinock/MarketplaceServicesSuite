using Carting.Core.CartAggregate;
using Carting.Core.Interfaces;
using MongoDB.Driver;

namespace Carting.Infrastructure.Data;

public class CartRepository : ICartRepository
{
    private readonly IMongoCollection<Cart> _collection;

    public CartRepository(AppDbContext context)
    {
        _collection = context.Carts;
    }

    public async Task<IEnumerable<Cart>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<Cart?> GetByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Cart> AddCartAsync(Cart cart)
    {
        await _collection.InsertOneAsync(cart);
        return cart;
    }

    public async Task<Cart> AddCartAsync(string id)
    {
        var cart = new Cart
        {
            Id = id
        };

        await _collection.InsertOneAsync(cart);

        return cart;
    }

    public async Task<Cart?> AddItemAsync(string id, Item item)
    {
        var cart = await GetByIdAsync(id);

        if (cart != null)
        {
            cart.Items.Add(item);
            await _collection.ReplaceOneAsync(x => x.Id == id, cart);
        }

        return cart;
    }

    public async Task<Cart?> RemoveItemAsync(string cartId, int itemId)
    {
        var cart = await GetByIdAsync(cartId);

        if (cart != null)
        {
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item is not null)
            {
                cart.Items.Remove(item);
                await _collection.ReplaceOneAsync(x => x.Id == cartId, cart);
            }
        }

        return cart;
    }

    public async Task<Cart?> RemoveItemsAsync(string cartId, IEnumerable<int> itemIds)
    {
        var cart = await GetByIdAsync(cartId);

        if (cart != null)
        {
            var items = cart.Items.Where(x => itemIds.Contains(x.Id)).ToList();

            foreach (var item in items)
            {
                cart.Items.Remove(item);
            }

            await _collection.ReplaceOneAsync(x => x.Id == cartId, cart);
        }

        return cart;
    }

    public async Task UpdateCartAsync(Cart cart)
    {
        await _collection.ReplaceOneAsync(x => x.Id == cart.Id, cart);
    }

    public async Task<IEnumerable<Cart>> GetAllByItemIdAsync(int itemId)
    {
        var carts = await _collection.Find(_ => true).ToListAsync();
        return carts.Where(cart => cart.Items.Any(item => item.Id == itemId));
    }

    public async Task<IEnumerable<Cart>> GetAllByItemIdsAsync(IEnumerable<int> itemIds)
    {
        var filter = Builders<Cart>.Filter.ElemMatch(x => x.Items, x => itemIds.Contains(x.Id));
        return await _collection.Find(filter).ToListAsync();
    }
}
