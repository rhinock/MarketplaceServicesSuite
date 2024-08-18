using Ardalis.Result;
using Ardalis.SharedKernel;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.Items.Update;

public record UpdateItemCommand(
    int Id,
    string Name,
    int CategoryId,
    decimal Price,
    int Amount,
    string? Description,
    string? Image)
    : ICommand<Result<ItemResponse>>;