using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.Catalog.Entities;
public class Category : AggregateRoot<long>
{
    public const int MaxDepth = 4;

    private readonly List<Category> _subCategories = new();

    public string Name { get; private set; }
    public long? ParentCategoryId { get; private set; }
    /// <summary>
    /// Path of the parent category, excluding this category's own ID.
    /// </summary>
    public string? Path { get; private set; }
    /// <summary>
    /// Collection of children categories.
    /// </summary>
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

        ValidatePathConsistency(parentCategoryId, path);

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
        if (id <= 0)
        {
            throw new ArgumentException("ID must be a positive number.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }

        ValidatePathConsistency(parentCategoryId, path, id);

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
    /// Updates the Name and ParentCategoryId of the Category.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parentCategoryId"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Update(string name, long? parentCategoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name is invalid.", nameof(name));
        }

        if (parentCategoryId.HasValue && parentCategoryId.Value == Id)
        {
            throw new InvalidOperationException("A category cannot be its own parent.");
        }

        Name = name;
        ParentCategoryId = parentCategoryId;
    }

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
        EnsureCanAddSubCategory();

        if (subCategory == null)
        {
            throw new ArgumentNullException(nameof(subCategory));
        }

        if (subCategory.ParentCategoryId != Id)
        {
            throw new InvalidOperationException("Subcategory's parent ID does not match this category's ID.");
        }

        if (_subCategories.Contains(subCategory))
        {
            throw new InvalidOperationException("Subcategory already exists in this category.");
        }

        _subCategories.Add(subCategory);
    }

    /// <summary>
    /// Adds a collection of subcategories to this category.
    /// </summary>
    /// <param name="subCategories"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddSubCategories(IEnumerable<Category> subCategories)
    {
        EnsureCanAddSubCategory();

        if (subCategories == null)
        {
            throw new ArgumentNullException(nameof(subCategories));
        }

        foreach (var subCategory in subCategories)
        {
            if (subCategory.ParentCategoryId != Id)
            {
                throw new InvalidOperationException(
                    $"Subcategory '{subCategory.Name}' has mismatched ParentCategoryId: expected {Id}, got {subCategory.ParentCategoryId}."
                );
            }

            AddSubCategory(subCategory);
        }
    }

    /// <summary>
    /// Updates the path of this category.
    /// </summary>
    /// <param name="newPath"></param>
    public void UpdatePath(string newPath)
    {
        ValidatePathConsistency(ParentCategoryId, newPath, Id);
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

    /// <summary>
    /// Returns the depth of the current Category.
    /// </summary>
    public int Depth => string.IsNullOrWhiteSpace(Path) ? 0 : Path.Split('/', StringSplitOptions.RemoveEmptyEntries).Length;

    /// <summary>
    /// Calculated the remainig depth under the category.
    /// </summary>
    public int RemainingDepth => MaxDepth - Depth;

    /// <summary>
    /// Checks if subcategories cand be added.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    private void EnsureCanAddSubCategory()
    {
        if (RemainingDepth <= 0)
        {
            throw new InvalidOperationException("Max category depth reached.");
        }
    }

    private static void ValidatePathConsistency(long? parentCategoryId, string? path, long? categoryId = null)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        var ids = ParsePath(path);

        ValidatePathDepth(ids);
        ValidatePathNoDuplicates(ids);
        ValidatePathNoSelfReference(ids, categoryId);
        ValidatePathParentConsistency(ids, parentCategoryId);
    }

    private static List<long> ParsePath(string path) =>
        path.Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

    private static void ValidatePathDepth(List<long> ids)
    {
        if (ids.Count > MaxDepth)
        {
            throw new ArgumentException($"Path exceeds maximum allowed depth of {MaxDepth}.");
        }
    }

    private static void ValidatePathNoDuplicates(List<long> ids)
    {
        var duplicates = ids.GroupBy(id => id)
                            .Where(g => g.Count() > 1)
                            .Select(g => g.Key)
                            .ToList();

        if (duplicates.Any())
        {
            throw new ArgumentException($"Path contains duplicate IDs: {string.Join(", ", duplicates)}.");
        }
    }

    private static void ValidatePathNoSelfReference(List<long> ids, long? categoryId)
    {
        if (categoryId.HasValue && ids.Contains(categoryId.Value))
        {
            throw new ArgumentException($"Path cannot contain the category's own ID ({categoryId.Value}).");
        }
    }

    private static void ValidatePathParentConsistency(List<long> ids, long? parentCategoryId)
    {
        if (parentCategoryId.HasValue)
        {
            var expectedParentId = ids.LastOrDefault();
            if (expectedParentId != parentCategoryId.Value)
            {
                throw new ArgumentException(
                    $"Inconsistent hierarchy: path ends with {expectedParentId}, " +
                    $"but parentCategoryId is {parentCategoryId.Value}."
                );
            }
        }
    }




}
