using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using Catalog.Core.Categories;
using Catalog.Core.Exceptions;
using Catalog.Core.Items;
using Catalog.UseCases.Invariants;
using Catalog.UseCases.Responses;
using Common.RabbitMq;

namespace Catalog.UseCases.Items.Update;

public class UpdateItemHandler(
        IRepository<Item> _itemRepository,
        IRepository<Category> _categoryRepository,
        IMapper _mapper,
        RabbitMqClient _rabbitMqClient)
    : ICommandHandler<UpdateItemCommand, Result<ItemResponse>>
{
    public async Task<Result<ItemResponse>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var existingItem =
            await _itemRepository.GetByIdAsync(request.Id, cancellationToken) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.ItemNotFound, request.Id));

        var _ =
            await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.CategoryNotFound, request.CategoryId));

        _mapper.Map(request, existingItem);
        await _itemRepository.UpdateAsync(existingItem, cancellationToken);

        var message = _mapper.Map<ItemUpdateMessage>(existingItem);
        _rabbitMqClient.Publish(message);

        var response = _mapper.Map<ItemResponse>(existingItem);

        return Result.Success(response);
    }
}