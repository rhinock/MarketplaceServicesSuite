using Ardalis.SharedKernel;
using Catalog.Core.Categories;
using Catalog.Core.Categories.Specifications;
using Catalog.Core.Interfaces;

namespace Catalog.Infrastructure.Services;

public class CategoryService(IRepository<Category> _repository) : ICategoryService
{
    public async Task<IEnumerable<int>> GetCategoryAndChildrenIdsAsync(
        int id, CancellationToken cancellationToken)
    {
        var specification = new CategoryByIdSpecification(id);
        var category = await _repository.FirstOrDefaultAsync(specification, cancellationToken);

        if (category == null)
        {
            return [];
        }

        var categoryIds = new List<int> { category.Id };

        foreach (var childCategory in category.ChildCategories)
        {
            categoryIds.AddRange(await GetCategoryAndChildrenIdsAsync(childCategory.Id, cancellationToken));
        }

        return categoryIds;
    }

    public async Task<bool> HasCircularReferenceAsync(
        Category existingEntity,
        int? requestParentId,
        CancellationToken cancellationToken)
    {
        if (requestParentId.HasValue)
        {
            var categoryAndChildrenIds = await GetCategoryAndChildrenIdsAsync(existingEntity.Id, cancellationToken);

            if (categoryAndChildrenIds.Contains(requestParentId.Value))
            {
                return true;
            }
        }

        return false;
    }

    public bool HasSelfReference(int requestId, int? requestParentId)
    {
        if (requestParentId.HasValue)
        {
            if (requestId == requestParentId.Value)
            {
                return true;
            }
        }

        return false;
    }
}