namespace Ogma.Api.Contracts.Catalog;

public record CategoryWithAncestorsResponse(long Id, string Name, long? ParentCategoryId, string? Path, IReadOnlyCollection<CategoryResponse> Ancestors);
