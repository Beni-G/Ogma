namespace Ogma.Api.Contracts.Catalog;

public record CategoryWithDescendantsResponse(long Id, string Name, long? ParentCategoryId, string? Path, IReadOnlyCollection<CategoryWithDescendantsResponse> SubCategories);

