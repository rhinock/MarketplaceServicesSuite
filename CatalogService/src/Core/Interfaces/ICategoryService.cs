using Catalog.Core.Categories;

namespace Catalog.Core.Interfaces;

public interface ICategoryService
{
    Task<bool> HasCircularReferenceAsync(
        Category existingEntity,
        int? requestParentId,
        CancellationToken cancellationToken);

    bool HasSelfReference(
        int requestId,
        int? requestParentId);

    Task<IEnumerable<int>> GetCategoryAndChildrenIdsAsync(
        int id,
        CancellationToken cancellationToken);
}