using Ardalis.Result;
using Ardalis.SharedKernel;
using Carting.Core.Exceptions;
using Carting.Core.Interfaces;
using Carting.UseCases.Invariants;

namespace Carting.UseCases.Carts.Delete;

public class DeleteItemFromCartHandler(ICartRepository _repository)
    : ICommandHandler<DeleteItemFromCartCommand, Result>
{
    public async Task<Result> Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart =
            await _repository.GetByIdAsync(request.CartId) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.CartNotFound, request.CartId));

        var item =
            cart.Items.FirstOrDefault(x => x.Id == request.ItemId) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.ItemNotFound, request.ItemId));

        await _repository.RemoveItemAsync(cart.Id, item.Id);

        return Result.Success();
    }
}