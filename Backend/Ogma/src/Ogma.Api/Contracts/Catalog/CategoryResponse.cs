namespace Ogma.Api.Contracts.Catalog;

public record CategoryResponse(
    long Id, 
    string Name, 
    long? ParentCategoryId, 
    string? Path,
    IReadOnlyCollection<CategoryResponse>? Ancestors = null,
    IReadOnlyCollection<CategoryResponse>? SubCategories = null);

