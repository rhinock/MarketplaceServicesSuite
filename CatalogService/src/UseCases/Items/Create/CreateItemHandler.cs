using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using Catalog.Core.Categories;
using Catalog.Core.Exceptions;
using Catalog.Core.Items;
using Catalog.UseCases.Invariants;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.Items.Create;

public class CreateItemHandler(
    IRepository<Item> _itemRepository,
    IRepository<Category> _categoryRepository,
    IMapper _mapper)
    : ICommandHandler<CreateItemCommand, Result<ItemResponse>>
{
    public async Task<Result<ItemResponse>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var existingItem = await _itemRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingItem != null)
        {
            throw new EntityExistsException(string.Format(ErrorMessages.ItemExists, request.Id));
        }

        var _ =
            await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.CategoryNotFound, request.CategoryId));

        var newEntity = _mapper.Map<Item>(request);
        await _itemRepository.AddAsync(newEntity, cancellationToken);
        var response = _mapper.Map<ItemResponse>(newEntity);

        return response;
    }
}
