using Ardalis.Result;
using Ardalis.SharedKernel;
using Carting.Core.Interfaces;
using Carting.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Carting.UseCases.Carts.Get.V1;

public record GetCartQueryV1 : IGetCartQuery, IQuery<Result<CartResponse>>
{
    [FromRoute(Name = "id")] public required string Id { get; set; }
}