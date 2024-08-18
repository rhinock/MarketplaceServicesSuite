using Carting.Responses;
using Carting.UseCases.Carts.Add;
using Carting.UseCases.Carts.Delete;
using Carting.UseCases.Carts.Get.V1;
using Carting.Web.Invariants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carting.Web.Controllers.V1;

[Authorize(Policies.AllPolicies)]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/carts")]
[ApiController]
public class CartsControllerV1(IMediator _mediator) : ControllerBase
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
            new { version = "1.0", id = response.Id },
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

    [HttpGet("{id}", Name = RouteNames.GetCartByIdAsyncV1)]
    public async Task<ActionResult<CartResponse>> GetCartByIdAsync(
        [FromRoute] GetCartQueryV1 request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result.Value);
    }
}