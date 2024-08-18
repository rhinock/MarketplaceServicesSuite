using Ardalis.SharedKernel;
using Catalog.Core.Categories;

namespace Catalog.Core.Items;

public class Item : EntityBase, IAggregateRoot
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }

    #region Navigation Properties

    public Category Category { get; set; } = null!;
    public int CategoryId { get; set; }

    #endregion
}