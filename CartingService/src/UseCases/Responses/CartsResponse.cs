namespace Carting.Responses;

public record CartResponse(string Id, ICollection<ItemResponse> Items);