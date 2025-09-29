using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.Catalog.Entities;
public class Category : AggregateRoot<long>
{
    public string Name { get; private set; }
    public long? ParentCategoryId { get; private set; }
    /// <summary>
    /// Path of the parent category, excluding this category's own ID.
    /// </summary>
    public string? Path { get; private set; }

    private readonly List<Category> _subCategories = new();
    public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

    /// <summary>
    /// Creates a new instance of the Category class with the specified name, optional parent category ID, and path.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parentCategoryId"></param>
    /// <param name="path"></param>
    /// <exception cref="ArgumentException"></exception>
    private Category(string name, long? parentCategoryId = null, string? path = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }
        Name = name;
        ParentCategoryId = parentCategoryId;
        Path = path;
    }

    /// <summary>
    /// Creates a new instance of the Category class with the specified id, name, optional parent category ID, and path.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="parentCategoryId"></param>
    /// <param name="path"></param>
    /// <exception cref="ArgumentException"></exception>
    private Category(long id, string name, long? parentCategoryId = null, string? path = null) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }
        Name = name;
        ParentCategoryId = parentCategoryId;
        Path = path;
    }

    /// <summary>
    /// Creates a new Category instance.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parentCategoryId"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Category Create(string name, long? parentCategoryId = null, string? path = null) => new(name, parentCategoryId, path);

    /// <summary>
    /// Reconstitutes a Category instance from existing data.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="parentCategoryId"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Category Reconstitute(long id, string name, long? parentCategoryId = null, string? path = null) => new(id, name, parentCategoryId, path);

    /// <summary>
    /// Adds a subcategory to this category.
    /// </summary>
    /// <param name="subCategory"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>

    /// <summary>
    /// Computes the full path including this category's ID.
    /// </summary>
    public string GetFullPath() => string.IsNullOrEmpty(Path) ? Id.ToString() : $"{Path}/{Id}";

    /// <summary>
    /// Adds a subcategory to this category.
    /// </summary>
    /// <param name="subCategory"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddSubCategory(Category subCategory)
    {
        if (subCategory == null)
        {
            throw new ArgumentNullException(nameof(subCategory));
        }
        if (subCategory.ParentCategoryId != Id)
        {
            throw new InvalidOperationException("Subcategory's parent ID does not match this category's ID.");
        }
        _subCategories.Add(subCategory);
    }

    /// <summary>
    /// Updates the path of this category.
    /// </summary>
    /// <param name="newPath"></param>
    public void UpdatePath(string newPath)
    {
        Path = newPath;
    }

    /// <summary>
    /// Gets all descendant categories recursively.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Category> GetAllDescendants()
    {
        foreach (var sub in _subCategories)
        {
            yield return sub;

            foreach (var child in sub.GetAllDescendants())
            {
                yield return child;
            }
        }
    }

    /// <summary>
    /// Checks if this category is a root category (i.e., has no parent).
    /// </summary>
    /// <returns></returns>
    public bool IsRootCategory() => ParentCategoryId == null;
}
