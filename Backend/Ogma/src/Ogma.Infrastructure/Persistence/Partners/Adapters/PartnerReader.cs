using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Ports;
using Ogma.Infrastructure.Persistence.Partners.Contexts;
using Ogma.Infrastructure.Persistence.Partners.Extensions;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Partners.Adapters;

public class PartnerReader : IPartnerReader
{
    private readonly PartnersDbContext _partnersDbContext;
    private readonly IMapper _mapper;

    public PartnerReader(PartnersDbContext partnersDbContext, IMapper mapper)
    {
        _partnersDbContext = partnersDbContext;
        _mapper = mapper;
    }

    public async Task<PartnerDto?> GetByIdAsync(long id)
    {
        var entity = await _partnersDbContext.Partners
            .Include(p => p.Identifiers)
            .Include(p => p.BankAccounts)
            .Include(p => p.Contacts)
            .Include(p => p.Roles)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        return entity?.ToDto();
    }

    public async Task<IDictionary<long, PartnerDto>> GetByIdsAsync(IEnumerable<long> ids)
    {
        return await _partnersDbContext.Partners
            .Include(p => p.Identifiers)
            .Include(p => p.BankAccounts)
            .Include(p => p.Contacts)
            .Include(p => p.Roles)
            .AsNoTracking()
            .Where(p => ids.Contains(p.Id))
            .ToDictionaryAsync(
                keySelector: p => p.Id,
                elementSelector: p => p.ToDto()
            );
    }

    public async Task<IEnumerable<PartnerDto>> GetAllAsync()
    {
        return await _partnersDbContext.Partners
            .Include(p => p.Identifiers)
            .Include(p => p.BankAccounts)
            .Include(p => p.Contacts)
            .Include(p => p.Roles)
            .AsNoTracking()
            .Select(p => p.ToDto())
            .ToListAsync();
    }

    public async Task<IEnumerable<PartnerDto>> GetAllAsync(Expression<Func<PartnerDto, bool>> predicate)
    {
        var modelPredicate = _mapper.MapExpression<Expression<Func<Models.Partner, bool>>>(predicate);
        return await _partnersDbContext.Partners
            .Include(p => p.Identifiers)
            .Include(p => p.BankAccounts)
            .Include(p => p.Contacts)
            .Include(p => p.Roles)
            .AsNoTracking()
            .Where(modelPredicate)
            .Select(p => p.ToDto())
            .ToListAsync();
    }
}
