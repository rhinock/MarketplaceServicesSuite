using Ardalis.Result;
using Ardalis.SharedKernel;
using Catalog.UseCases.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.UseCases.Items.Get;

public record GetItemQuery : IQuery<Result<ItemResponse>>
{
    [FromRoute(Name = "id")] public required int Id { get; set; }
}