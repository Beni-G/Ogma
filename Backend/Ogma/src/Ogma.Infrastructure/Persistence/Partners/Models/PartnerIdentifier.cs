namespace Ogma.Infrastructure.Persistence.Partners.Models;

public class PartnerIdentifier
{
    public long Id { get; set; }
    public long PartnerId { get; set; }
    public Partner Partner { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;
    public DateTime? ValidityStart { get; set; }
    public DateTime? ValidityEnd { get; set; }
    public bool IsPrimary { get; set; }
}
