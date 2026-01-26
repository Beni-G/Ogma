using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.Orders.ValueObjects;

public class OrderPartner : ValueObject
{
    public long PartnerId { get; }
    public string PartnerName { get; }

    public OrderPartner(long partnerId, string partnerName)
    {
        PartnerId = partnerId;
        PartnerName = partnerName;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PartnerId;
        yield return PartnerName;
    }

    public override string ToString() => $"{PartnerName} (ID: {PartnerId})";
}
