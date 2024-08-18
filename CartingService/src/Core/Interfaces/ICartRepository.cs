using Carting.Core.CartAggregate;

namespace Carting.Core.Interfaces;

public interface ICartRepository
{
    Task<IEnumerable<Cart>> GetAllAsync();
    Task<Cart?> GetByIdAsync(string id);
    Task<Cart> AddCartAsync(Cart cart);
    Task<Cart> AddCartAsync(string id);
    Task<Cart?> AddItemAsync(string cartId, Item item);
    Task<Cart?> RemoveItemAsync(string cartId, int itemId);
    Task<Cart?> RemoveItemsAsync(string cartId, IEnumerable<int> itemIds);
    Task<IEnumerable<Cart>> GetAllByItemIdAsync(int itemId);
    Task<IEnumerable<Cart>> GetAllByItemIdsAsync(IEnumerable<int> itemIds);
    Task UpdateCartAsync(Cart cart);
}