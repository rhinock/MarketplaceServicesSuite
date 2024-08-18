using Ardalis.Result;
using Ardalis.SharedKernel;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.Categories.List;

public record ListCategoriesQuery(
    int? Skip,
    int? Take)
    : IQuery<Result<List<CategoryResponse>>>;