using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace Carting.UseCases.Carts.Delete;

public record DeleteItemFromCartCommand : ICommand<Result>
{
    [FromRoute(Name = "cartId")] public required string CartId { get; set; }
    [FromRoute(Name = "itemId")] public required int ItemId { get; set; }
}