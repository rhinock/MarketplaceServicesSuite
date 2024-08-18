using Ardalis.SharedKernel;
using Catalog.Core.Items;

namespace Catalog.Core.Categories;

public class Category : EntityBase, IAggregateRoot
{
    public string Name { get; set; } = null!;
    public string? Image { get; set; }

    #region Navigation Properties

    public ICollection<Item> Items { get; set; } = [];
    public ICollection<Category> ChildCategories { get; set; } = [];

    public Category? Parent { get; set; }
    public int? ParentId { get; set; }

    #endregion
}
