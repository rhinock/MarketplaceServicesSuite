namespace Catalog.UseCases.Responses;

public record ItemResponse(
    int Id,
    string Name,
    decimal Price,
    int Amount,
    int CategoryId,
    string? Description,
    string? Image
) : BaseResponse([]);