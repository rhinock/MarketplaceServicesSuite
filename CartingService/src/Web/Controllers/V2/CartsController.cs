using Carting.Responses;
using Carting.UseCases.Carts.Add;
using Carting.UseCases.Carts.Delete;
using Carting.UseCases.Carts.Get.V2;
using Carting.Web.Invariants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carting.Web.Controllers.V2;

[Authorize(Policies.Buyer)]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/carts")]
[ApiController]
public class CartsControllerV2(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CartResponse>> AddItemToCartAsync(
        [FromBody] AddItemToCartCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        var response = result.Value;

        return CreatedAtRoute(
            RouteNames.GetCartByIdAsyncV1,
            new { version = "2.0", id = response.Id },
            response);
    }

    [HttpDelete("{cartId}/items/{itemId}")]
    public async Task<ActionResult> DeleteItemFromCartAsync(
        [FromRoute] DeleteItemFromCartCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id}", Name = RouteNames.GetCartByIdAsyncV2)]
    public async Task<ActionResult<ICollection<ItemResponse>>> GetCartByIdAsync(
        [FromRoute] GetCartQueryV2 request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result.Value);
    }
}