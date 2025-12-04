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

    public async Task<Partner?> GetByIdAsync(int id)
    {
        var entity = await _partnersDbContext.Partners
            .Include(p => p.Identifiers)
            .Include(p => p.Roles)
            .Include(p => p.BankAccounts)
            .Include(p => p.Contacts)
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
            .Where(modelPredicate)
            .Select(p => p.ToDomain())
            .ToListAsync();
    }

    public async Task<Partner> AddAsync(Partner partner)
    {
        var model = partner.ToModel();
        await _partnersDbContext.Partners.AddAsync(model);
        await _partnersDbContext.SaveChangesAsync();

        return model.ToDomain();
    }

    public async Task<bool> UpdateAsync(Partner partner)
    {
        var model = partner.ToModel();
        _partnersDbContext.Partners.Attach(model);
        _partnersDbContext.Entry(model).State = EntityState.Modified;

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


}
