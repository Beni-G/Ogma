using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.Catalog.Queries;
public class GetAllCategoriesFlatHandler : IRequestHandler<GetAllCategoriesFlatQuery, List<CategoryWithDescendantsDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesFlatHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryWithDescendantsDto>> Handle(GetAllCategoriesFlatQuery query, CancellationToken cancellationToken)
    {
        return (await _categoryRepository.GetAllAsync())
            .Select(c => c.ToDtoWithDescendants())
            .ToList();
    }
}
