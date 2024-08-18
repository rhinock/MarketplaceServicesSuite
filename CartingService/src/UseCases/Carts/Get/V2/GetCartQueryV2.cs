using Ardalis.Result;
using Ardalis.SharedKernel;
using Carting.Core.Interfaces;
using Carting.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Carting.UseCases.Carts.Get.V2;

public record GetCartQueryV2() : IGetCartQuery, IQuery<Result<ICollection<ItemResponse>>>
{
    [FromRoute(Name = "id")] public required string Id { get; set; }
}