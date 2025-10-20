using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogma.Application.Catalog.Queries;
public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryWithDescendantsDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryWithDescendantsDto> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetByIdAsync(query.Id);
        if (result == null)
        {
            throw new KeyNotFoundException($"Category with ID {query.Id} was not found.");

        }
        return result.ToDtoWithDescendants();
    }
}
