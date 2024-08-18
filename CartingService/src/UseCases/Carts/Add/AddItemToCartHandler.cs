using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using Carting.Core.Exceptions;
using Carting.Core.Interfaces;
using Carting.Responses;
using Carting.UseCases.Invariants;

namespace Carting.UseCases.Carts.Add;

public class AddItemToCartHandler(ICartRepository _repository, IMapper _mapper)
    : ICommandHandler<AddItemToCartCommand, Result<CartResponse>>
{
    public async Task<Result<CartResponse>> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _repository.GetByIdAsync(request.Id);
        cart ??= await _repository.AddCartAsync(request.Id);

        var item = cart.Items.FirstOrDefault(x => x.Id == request.Item.Id);
        if (item is not null)
        {
            throw new EntityExistsException(string.Format(ErrorMessages.ItemExists, request.Item.Id));
        }

        cart = await _repository.AddItemAsync(cart.Id, request.Item);
        var response = _mapper.Map<CartResponse>(cart);

        return Result.Success(response);
    }
}