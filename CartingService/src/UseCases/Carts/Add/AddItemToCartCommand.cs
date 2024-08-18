using Ardalis.Result;
using Ardalis.SharedKernel;
using Carting.Core.CartAggregate;
using Carting.Responses;

namespace Carting.UseCases.Carts.Add;

public record AddItemToCartCommand(string Id, Item Item) : ICommand<Result<CartResponse>>;