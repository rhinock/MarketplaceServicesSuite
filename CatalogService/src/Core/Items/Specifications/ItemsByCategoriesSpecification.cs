using Ardalis.Specification;

namespace Catalog.Core.Items.Specifications;

public class ItemsByCategoriesSpecification : Specification<Item>
{
    public ItemsByCategoriesSpecification(IEnumerable<int> categoryIds)
    {
        Query.Where(x => categoryIds.Contains(x.CategoryId));
    }
}