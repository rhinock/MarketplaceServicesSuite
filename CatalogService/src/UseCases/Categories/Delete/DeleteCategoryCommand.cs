using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.UseCases.Categories.Delete;

public record DeleteCategoryCommand : ICommand<Result>
{
    [FromRoute(Name = "id")] public required int Id { get; set; }
}