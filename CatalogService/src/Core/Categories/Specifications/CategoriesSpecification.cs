using Ardalis.Specification;

namespace Catalog.Core.Categories.Specifications;

public class CategoriesSpecification : Specification<Category>
{
    public CategoriesSpecification(int? skip, int? take)
    {
        if (skip.HasValue)
        {
            Query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            Query.Take(take.Value);
        }

        Query
            .Include(x => x.Items)
            .Include(x => x.ChildCategories);
    }
}
