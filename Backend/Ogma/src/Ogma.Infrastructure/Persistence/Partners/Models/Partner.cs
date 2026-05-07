using Ogma.Infrastructure.Persistence.SharedKernel.BaseTypes;
using Ogma.Infrastructure.Persistence.SharedKernel.ValueObjectRecords;

namespace Ogma.Infrastructure.Persistence.Partners.Models;

public class Partner : Entity
{
    public string? IndividualFirstName { get; set; }
    public string? IndividualLastName { get; set; }
    public string? CompanyName { get; set; }
    public bool IsNaturalPerson { get; set; } = false;
    public bool IsActive { get; set; }
    public string? DisplayName { get; set; }
    public AddressRecord? HQAddress { get; set; }
    public List<PartnerRoleType> Roles { get; set; } = new();
    public List<PartnerIdentifier> Identifiers { get; set; } = new();
    public List<PartnerBankAccount> BankAccounts { get; set; } = new();
    public List<PartnerContact> Contacts { get; set; } = new();
}
