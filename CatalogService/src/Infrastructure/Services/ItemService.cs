using Ardalis.SharedKernel;
using Catalog.Core.Interfaces;
using Catalog.Core.Items;
using Catalog.Core.Items.Specifications;

namespace Catalog.Infrastructure.Services;

public class ItemService(IRepository<Item> _repository) : IItemService
{
    public async Task<IEnumerable<int>> GetCategoryAndChildrenItemIdsAsync(
        IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var specification = new ItemsByCategoriesSpecification(ids);
        var items = await _repository.ListAsync(specification, cancellationToken);
        return items.Select(x => x.Id);
    }
}