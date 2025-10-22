namespace Ogma.Api.Contracts.Catalog;

public record CreateCategoryRequest(string Name, long? ParentCategoryId = null);

