using Ogma.Infrastructure.Persistence.Partners.ValueObjectRecords;

namespace Ogma.Infrastructure.Persistence.Partners.Models;

public class PartnerBankAccount
{
    public long Id { get; set; }
    public long PartnerId { get; set; }
    public Partner Partner { get; set; }
    public BankAccountRecord BankAccount { get; set; } = default!;
    public bool IsDefault { get; set; }
}
