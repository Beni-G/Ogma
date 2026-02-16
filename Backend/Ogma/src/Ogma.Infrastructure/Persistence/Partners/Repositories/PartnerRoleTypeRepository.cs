using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;
using Ogma.Infrastructure.Persistence.Partners.Contexts;
using Ogma.Infrastructure.Persistence.Partners.Extensions;

namespace Ogma.Infrastructure.Persistence.Partners.Repositories;

public class PartnerRoleTypeRepository : IPartnerRoleTypeRepository
{
    private readonly PartnersDbContext _partnersDbContext;

    public PartnerRoleTypeRepository(PartnersDbContext partnersDbContext) => _partnersDbContext = partnersDbContext;

    public async Task<PartnerRoleType?> GetByIdAsync(long id)
    {
        var entity = await _partnersDbContext.PartnerRoleTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(prt => prt.Id == id);
        return entity?.ToDomain();
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
