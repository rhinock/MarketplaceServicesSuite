using Ardalis.Result;
using Ardalis.SharedKernel;
using Catalog.Core.Categories;
using Catalog.Core.Exceptions;
using Catalog.Core.Interfaces;
using Catalog.UseCases.Invariants;
using Common.RabbitMq;

namespace Catalog.UseCases.Categories.Delete;

public class DeleteCategoryHandler(
    IRepository<Category> _repository,
    ICategoryService _categoryService,
    IItemService _itemService,
    RabbitMqClient _rabbitMqClient)
    : ICommandHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity =
            await _repository.GetByIdAsync(request.Id, cancellationToken) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.CategoryNotFound, request.Id));

        var categoryIds = await _categoryService.GetCategoryAndChildrenIdsAsync(entity.Id, cancellationToken);
        var itemIds = await _itemService.GetCategoryAndChildrenItemIdsAsync(categoryIds, cancellationToken);

        await _repository.DeleteAsync(entity, cancellationToken);

        var message = new ItemDeleteMessage { Ids = itemIds };
        _rabbitMqClient.Publish(message);

        return Result.Success();
    }
}
