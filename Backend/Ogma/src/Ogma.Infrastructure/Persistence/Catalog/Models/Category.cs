namespace Ogma.Infrastructure.Persistence.Catalog.Models;
public class Category
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public long? ParentCategoryId { get; set; }
    public Category ParentCategory { get; set; } = default!;
    public List<Category> SubCategories { get; set; } = new();
    /// <summary>
    /// Path of the parent category, excluding this category's own ID.
    /// </summary>
    public string? Path { get; set; }
}
