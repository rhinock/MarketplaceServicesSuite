namespace Catalog.Core.Interfaces;

public interface IItemService
{
    Task<IEnumerable<int>> GetCategoryAndChildrenItemIdsAsync(
        IEnumerable<int> ids,
        CancellationToken cancellationToken);
}