using Ogma.Infrastructure.Persistence.SharedKernel.BaseTypes;

namespace Ogma.Infrastructure.Persistence.Catalog.Models;
public class Category : Entity
{
    public string Name { get; set; } = default!;
    public long? ParentCategoryId { get; set; }
    public Category ParentCategory { get; set; } = default!;
    public List<Category> SubCategories { get; set; } = new();
    /// <summary>
    /// Path of the parent category, excluding this category's own ID.
    /// </summary>
    public string? Path { get; set; }
}
