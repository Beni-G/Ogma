using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;
using Ogma.Infrastructure.Persistence.Partners.Contexts;
using Ogma.Infrastructure.Persistence.Partners.Extensions;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Partners.Repositories;

public class PartnerRoleTypeRepository : IPartnerRoleTypeRepository
{
    private readonly PartnersDbContext _partnersDbContext;
    private readonly IMapper _mapper;

    public PartnerRoleTypeRepository(PartnersDbContext partnersDbContext, IMapper mapper)
    {
        _partnersDbContext = partnersDbContext;
        _mapper = mapper;
    }

    public async Task<PartnerRoleType?> GetByIdAsync(long id)
    {
        var entity = await _partnersDbContext.PartnerRoleTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(prt => prt.Id == id);
        return entity?.ToDomain();
    }

    public async Task<IEnumerable<PartnerRoleType>> GetAllAsync()
    {
        return await _partnersDbContext.PartnerRoleTypes
           .AsNoTracking()
           .Select(prt => prt.ToDomain())
           .ToListAsync();
    }

    public async Task<IEnumerable<PartnerRoleType>> GetAllAsync(Expression<Func<PartnerRoleType, bool>> predicate)
    {
        var modelPredicate = _mapper.MapExpression<Expression<Func<Models.PartnerRoleType, bool>>>(predicate);

        return await _partnersDbContext.PartnerRoleTypes
           .AsNoTracking()
           .Where(modelPredicate)
           .Select(prt => prt.ToDomain())
           .ToListAsync();
    }

    public async Task<PartnerRoleType> AddAsync(PartnerRoleType partnerRoleType)
    {
        var model = partnerRoleType.ToModel();
        await _partnersDbContext.PartnerRoleTypes.AddAsync(model);
        await _partnersDbContext.SaveChangesAsync();

        return model.ToDomain();
    }

    public async Task<bool> UpdateAsync(PartnerRoleType partnerRoleType)
    {
        var model = partnerRoleType.ToModel();
        _partnersDbContext.PartnerRoleTypes.Attach(model);
        _partnersDbContext.Entry(model).State = EntityState.Modified;

        var affected = await _partnersDbContext.SaveChangesAsync();
        return affected > 0;
    }

    public async Task DeleteAsync(PartnerRoleType partnerRoleType)
    {
        var model = partnerRoleType.ToModel();
        _partnersDbContext.PartnerRoleTypes.Attach(model);
        _partnersDbContext.PartnerRoleTypes.Remove(model);

        await _partnersDbContext.SaveChangesAsync();
    }
}
