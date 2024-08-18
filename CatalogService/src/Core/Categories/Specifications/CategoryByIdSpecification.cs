using Ardalis.Specification;

namespace Catalog.Core.Categories.Specifications;

public class CategoryByIdSpecification : Specification<Category>
{
    public CategoryByIdSpecification(int id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Items)
            .Include(x => x.ChildCategories);
    }
}