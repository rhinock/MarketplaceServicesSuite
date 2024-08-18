using Ardalis.Result;
using Ardalis.SharedKernel;
using Catalog.UseCases.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.UseCases.Categories.Get;

public record GetCategoryQuery : IQuery<Result<CategoryResponse>>
{
    [FromRoute(Name = "id")] public required int Id { get; set; }
}