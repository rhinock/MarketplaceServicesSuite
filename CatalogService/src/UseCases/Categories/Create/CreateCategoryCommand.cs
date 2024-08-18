using Ardalis.Result;
using Ardalis.SharedKernel;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.Categories.Create;

public record CreateCategoryCommand(int Id, string Name, string? Image, int? ParentId) : ICommand<Result<CategoryResponse>>;