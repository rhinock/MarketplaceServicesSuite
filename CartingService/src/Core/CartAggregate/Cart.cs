using Ardalis.SharedKernel;

namespace Carting.Core.CartAggregate;

public class Cart : IAggregateRoot
{
    public string Id { get; set; } = null!;
    public ICollection<Item> Items { get; set; } = [];
}