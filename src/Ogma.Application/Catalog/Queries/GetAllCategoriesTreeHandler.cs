using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.Catalog.Queries;
public class GetAllCategoriesTreeHandler : IRequestHandler<GetAllCategoriesTreeQuery, List<CategoryWithDescendantsDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesTreeHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryWithDescendantsDto>> Handle(GetAllCategoriesTreeQuery query, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllCategoriesTreeAsync();
        return categories.Select(c => c.ToDtoWithDescendants()).ToList();
    }
}
