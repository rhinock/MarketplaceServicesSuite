using Ardalis.Specification;

namespace Catalog.Core.Items.Specifications;

public class ItemsSpecification : Specification<Item>
{
    public ItemsSpecification(int? categoryId, int? skip, int? take)
    {
        if (categoryId.HasValue)
        {
            Query.Where(x => x.CategoryId == categoryId);
        }

        if (skip.HasValue)
        {
            Query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            Query.Take(take.Value);
        }

        Query.Include(x => x.Category);
    }
}
