using Ardalis.Result;
using Ardalis.SharedKernel;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.Categories.Update;

public record UpdateCategoryCommand(int Id, string Name, string? Image, int? ParentId)
    : ICommand<Result<CategoryResponse>>;