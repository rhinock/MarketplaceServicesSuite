using Ardalis.Result;
using Ardalis.SharedKernel;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.Items.List;

public record ListItemsQuery(
    int? CategoryId,
    int? Skip,
    int? Take)
    : IQuery<Result<List<ItemResponse>>>;