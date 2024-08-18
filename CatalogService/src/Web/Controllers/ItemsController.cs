using Catalog.UseCases.Items.Delete;
using Catalog.UseCases.Items.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Catalog.UseCases.Items.Get;
using Catalog.UseCases.Items.List;
using Catalog.UseCases.Items.Update;
using Catalog.UseCases.Responses;

namespace Catalog.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemsController(IMediator _mediator) : ControllerBase
{
    [HttpPost(Name = nameof(CreateItemAsync))]
    public async Task<ActionResult<ItemResponse>> CreateItemAsync(
        [FromBody] CreateItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        var response = result.Value;

        return CreatedAtRoute(
            nameof(GetItemByIdAsync),
            new { id = response.Id },
            result.Value);
    }

    [HttpDelete("{id}", Name = nameof(DeleteItemAsync))]
    public async Task<ActionResult> DeleteItemAsync(
        [FromRoute] DeleteItemCommand request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id}", Name = nameof(GetItemByIdAsync))]
    public async Task<ActionResult<ItemResponse>> GetItemByIdAsync(
        [FromRoute] GetItemQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        var response = result.Value;
        AddLinks(response);

        return Ok(response);
    }

    [HttpGet(Name = nameof(GetItemsAsync))]
    public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItemsAsync(
        [FromQuery] ListItemsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result.Value);
    }

    [HttpPut(Name = nameof(UpdateItemAsync))]
    public async Task<ActionResult<ItemResponse>> UpdateItemAsync(
        [FromBody] UpdateItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result.Value);
    }

    private void AddLinks(ItemResponse item)
    {
        item.Links.Add(new Link(
            Url.Link(nameof(GetItemByIdAsync), new { id = item.Id })!,
            "self",
            HttpMethod.Get.Method));

        item.Links.Add(new Link(
            Url.Link(nameof(DeleteItemAsync), new { id = item.Id })!,
            "Delete Item by Id",
            HttpMethod.Delete.Method));
    }
}