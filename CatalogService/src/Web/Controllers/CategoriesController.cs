using Catalog.UseCases.Categories.Create;
using Catalog.UseCases.Categories.Delete;
using Catalog.UseCases.Categories.Get;
using Catalog.UseCases.Categories.List;
using Catalog.UseCases.Categories.Update;
using Catalog.UseCases.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(IMediator _mediator) : ControllerBase
{
    [HttpPost(Name = nameof(CreateCategoryAsync))]
    public async Task<ActionResult<CategoryResponse>> CreateCategoryAsync(
        [FromBody] CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return CreatedAtRoute(
            nameof(GetCategoryByIdAsync),
            new { id = result.Value.Id },
            result.Value);
    }

    [HttpDelete("{id}", Name = nameof(DeleteCategoryAsync))]
    public async Task<ActionResult> DeleteCategoryAsync(
        [FromRoute] DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id}", Name = nameof(GetCategoryByIdAsync))]
    public async Task<ActionResult<CategoryResponse>> GetCategoryByIdAsync(
        [FromRoute] GetCategoryQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        var response = result.Value;
        AddLinks(response);

        return Ok(response);
    }

    [HttpGet(Name = nameof(GetCategoriesAsync))]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategoriesAsync(
        [FromQuery] ListCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result.Value);
    }

    [HttpPut(Name = nameof(UpdateCategoryAsync))]
    public async Task<ActionResult<CategoryResponse>> UpdateCategoryAsync(
        [FromBody] UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result.Value);
    }

    private void AddLinks(CategoryResponse category)
    {
        category.Links.Add(new Link(
            Url.Link(nameof(GetCategoryByIdAsync), new { id = category.Id })!,
            "self",
            HttpMethod.Get.Method));

        category.Links.Add(new Link(
            Url.Link(nameof(DeleteCategoryAsync), new { id = category.Id })!,
            "Delete Category by Id",
            HttpMethod.Delete.Method));
    }
}