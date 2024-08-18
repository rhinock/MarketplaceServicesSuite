namespace Catalog.UseCases.Responses;

public record CategoryResponse(
    int Id,
    string Name,
    ICollection<ItemResponse> Items,
    int? ParentId,
    string? Image
) : BaseResponse([]);