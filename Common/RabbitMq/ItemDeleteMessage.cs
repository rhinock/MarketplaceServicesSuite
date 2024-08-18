namespace Common.RabbitMq;

public record ItemDeleteMessage
{
    public IEnumerable<int> Ids { get; set; } = [];
}