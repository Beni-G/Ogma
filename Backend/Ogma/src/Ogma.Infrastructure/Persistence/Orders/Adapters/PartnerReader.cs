using Microsoft.EntityFrameworkCore;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;
using Ogma.Infrastructure.Persistence.Partners.Contexts;

namespace Ogma.Infrastructure.Persistence.Orders.Adapters;

public class PartnerReader : IPartnerReader
{
    private readonly PartnersDbContext _partnersDbContext;

    public PartnerReader(PartnersDbContext partnersDbContext) => _partnersDbContext = partnersDbContext;

    public async Task<OrderPartnerDto?> GetByIdAsync(long id)
    {
        var partner = await _partnersDbContext.Partners
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        var orderPartner = partner is null
            ? null
            : new OrderPartnerDto(
                partner.Id,
                partner.IsNaturalPerson
                    ? $"{partner.IndividualFirstName ?? ""} {partner.IndividualLastName ?? ""}".Trim()
                    : partner.CompanyName ?? "Unknown Company"
            );

        return orderPartner;
    }

    public async Task<IDictionary<long, OrderPartnerDto>> GetByIdsAsync(IEnumerable<long> ids)
    {
        return await _partnersDbContext.Partners
            .AsNoTracking()
            .Where(p => ids.Contains(p.Id))
            .Select(p => new OrderPartnerDto(
                p.Id,
                p.IsNaturalPerson
                    ? $"{p.IndividualFirstName ?? ""} {p.IndividualLastName ?? ""}".Trim()
                    : p.CompanyName ?? "Unknown Company"
            ))
            .ToDictionaryAsync(p => p.PartnerId);
    }
}
