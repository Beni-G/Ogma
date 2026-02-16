using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Ports;
using Ogma.Infrastructure.Persistence.Partners.Contexts;
using Ogma.Infrastructure.Persistence.Partners.Extensions;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Partners.Adapters;

public class PartnerRoleTypeReader : IPartnerRoleTypeReader
{
    private readonly PartnersDbContext _partnersDbContext;
    private readonly IMapper _mapper;

    public PartnerRoleTypeReader(PartnersDbContext partnersDbContext, IMapper mapper)
    {
        _partnersDbContext = partnersDbContext;
        _mapper = mapper;
    }

    public async Task<PartnerRoleTypeDto?> GetByIdAsync(long id)
    {
        var entity = await _partnersDbContext.PartnerRoleTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(prt => prt.Id == id);
        return entity?.ToDto();
    }

    public async Task<IDictionary<long, PartnerRoleTypeDto>> GetByIdsAsync(IEnumerable<long> ids)
    {
        return await _partnersDbContext.PartnerRoleTypes
            .AsNoTracking()
            .Where(prt => ids.Contains(prt.Id))
            .ToDictionaryAsync(
                keySelector: prt => prt.Id,
                elementSelector: prt => prt.ToDto()
            );
    }

    public async Task<IEnumerable<PartnerRoleTypeDto>> GetAllAsync()
    {
        return await _partnersDbContext.PartnerRoleTypes
            .AsNoTracking()
            .Select(prt => prt.ToDto())
            .ToListAsync();
    }

    public async Task<IEnumerable<PartnerRoleTypeDto>> GetAllAsync(Expression<Func<PartnerRoleTypeDto, bool>> predicate)
    {
        var modelPredicate = _mapper.MapExpression<Expression<Func<Models.PartnerRoleType, bool>>>(predicate);
        return await _partnersDbContext.PartnerRoleTypes
            .AsNoTracking()
            .Where(modelPredicate)
            .Select(prt => prt.ToDto())
            .ToListAsync();
    }
}
