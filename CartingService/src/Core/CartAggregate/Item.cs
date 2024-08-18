using Ardalis.SharedKernel;

namespace Carting.Core.CartAggregate;

public class Item : ValueObject
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Image? Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}