namespace Carting.Responses;

public record ItemResponse(int Id, string Name, decimal Price, int Quantity, ImageResponse? Image);