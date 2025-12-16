using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;
using Ogma.Infrastructure.Persistence.Partners.Contexts;
using Ogma.Infrastructure.Persistence.Partners.Extensions;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Partners.Repositories;

public class PartnerRepository : IPartnerRepository
{
    private readonly PartnersDbContext _partnersDbContext;
    private readonly IMapper _mapper;

    public PartnerRepository(PartnersDbContext partnersDbContext, IMapper mapper)
    {
        _partnersDbContext = partnersDbContext;
        _mapper = mapper;
    }

    public async Task<Partner?> GetByIdAsync(long id)
    {
        var entity = await _partnersDbContext.Partners
            .Include(p => p.Identifiers)
            .Include(p => p.Roles)
            .Include(p => p.BankAccounts)
            .Include(p => p.Contacts)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        return entity?.ToDomain();
    }

    public async Task<IEnumerable<Partner>> GetAllAsync()
    {
        return await _partnersDbContext.Partners
           .Include(p => p.Identifiers)
           .Include(p => p.Roles)
           .Include(p => p.BankAccounts)
           .Include(p => p.Contacts)
           .AsNoTracking()
           .Select(p => p.ToDomain())
           .ToListAsync();
    }


    public async Task<IEnumerable<Partner>> GetAllAsync(Expression<Func<Partner, bool>> predicate)
    {
        var modelPredicate = _mapper.MapExpression<Expression<Func<Models.Partner, bool>>>(predicate);
        return await _partnersDbContext.Partners
            .Include(p => p.Identifiers)
            .Include(p => p.Roles)
            .Include(p => p.BankAccounts)
            .Include(p => p.Contacts)
            .AsNoTracking()
            .Where(modelPredicate)
            .Select(p => p.ToDomain())
            .ToListAsync();
    }

    public async Task<Partner> AddAsync(Partner partner)
    {
        var model = partner.ToModel();
        foreach (var role in model.Roles)
        {
            _partnersDbContext.Entry(role).State = EntityState.Unchanged;
        }
        await _partnersDbContext.Partners.AddAsync(model);
        await _partnersDbContext.SaveChangesAsync();

        return model.ToDomain();
    }

    public async Task<bool> UpdateAsync(Partner partner)
    {
        var existingPartner = await _partnersDbContext.Partners
            .Include(p => p.Identifiers)
            .Include(p => p.BankAccounts)
            .Include(p => p.Contacts)
            .Include(p => p.Roles)
            .FirstOrDefaultAsync(p => p.Id == partner.Id);

        if (existingPartner == null)
        {
            return false;
        }

        // Update scalar properties
        var partnerModel = partner.ToModel();
        _partnersDbContext.Entry(existingPartner).CurrentValues.SetValues(partnerModel);

        // Replace owned value object
        existingPartner.HQAddress = partnerModel.HQAddress;

        // Replace child collections
        ReplaceCollection(existingPartner.Identifiers, partnerModel.Identifiers);
        ReplaceCollection(existingPartner.BankAccounts, partnerModel.BankAccounts);
        ReplaceCollection(existingPartner.Contacts, partnerModel.Contacts);

        // Replace many-to-many roles in one pass
        var newRoleIds = partner.Roles?.Select(r => r.Id).Distinct().ToList() ?? new List<long>();
        var currentRoleIds = existingPartner.Roles.Select(r => r.Id).ToList();

        // Remove old roles
        foreach (var roleId in currentRoleIds.Except(newRoleIds))
        {
            var role = existingPartner.Roles.First(r => r.Id == roleId);
            existingPartner.Roles.Remove(role);
        }

        // Add new roles
        var rolesToAdd = await _partnersDbContext.PartnerRoleTypes
            .Where(r => newRoleIds.Except(currentRoleIds).Contains(r.Id))
            .ToListAsync();

        foreach (var role in rolesToAdd)
        {
            existingPartner.Roles.Add(role);
        }

        var affected = await _partnersDbContext.SaveChangesAsync();
        return affected > 0;
    }

    public async Task DeleteAsync(Partner partner)
    {
        var model = partner.ToModel();
        _partnersDbContext.Partners.Attach(model);
        _partnersDbContext.Partners.Remove(model);

        await _partnersDbContext.SaveChangesAsync();
    }

    // Helper for owned/child collections (full replace)
    private static void ReplaceCollection<T>(ICollection<T> existing, IEnumerable<T>? newItems) where T : class
    {
        existing.Clear();

        if (newItems != null)
        {
            foreach (var item in newItems)
            {
                existing.Add(item);
            }
        }
    }
}
