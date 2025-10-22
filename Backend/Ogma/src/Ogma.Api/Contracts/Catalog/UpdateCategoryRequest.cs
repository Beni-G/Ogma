namespace Ogma.Api.Contracts.Catalog;

public record UpdateCategoryRequest(long Id, string Name, long? ParentCategoryId);
