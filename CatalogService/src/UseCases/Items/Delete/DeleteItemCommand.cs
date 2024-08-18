using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.UseCases.Items.Delete;

public record DeleteItemCommand : ICommand<Result>
{
    [FromRoute(Name = "id")] public required int Id { get; set;}
}