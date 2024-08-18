using Ardalis.Result;
using Ardalis.SharedKernel;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.Items.Create;

public record CreateItemCommand(
    int Id,
    string Name,
    decimal Price,
    int Amount,
    int CategoryId,
    string? Description,
    string? Image)
    : ICommand<Result<ItemResponse>>;