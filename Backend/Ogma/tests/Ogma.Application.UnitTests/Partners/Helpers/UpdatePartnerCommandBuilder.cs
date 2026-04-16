using Ogma.Application.Partners.Commands;
using Ogma.Application.Partners.Extensions;
using Ogma.Application.SharedKernel.Extensions;
using Ogma.Domain.Partners.Entities;

namespace Ogma.Application.UnitTests.Partners.Helpers;

public class UpdatePartnerCommandBuilder
{
    private UpdatePartnerCommand _command = null!;

    public UpdatePartnerCommandBuilder FromPartner(Partner partner)
    {
        _command = new UpdatePartnerCommand(
            Id: partner.Id,
            IndividualName: partner.IndividualName?.ToDto(),
            CompanyName: partner.CompanyName,
            IsNaturalPerson: partner.IsNaturalPerson,
            IsActive: partner.IsActive,
            DisplayName: partner.DisplayName,
            HQAddress: partner.HQAddress?.ToDto(),
            Identifiers: partner.Identifiers.Select(i => i.ToDto()).ToList(),
            RoleIds: partner.RoleIds.ToList(),
            BankAccounts: partner.BankAccounts.Select(b => b.ToDto()).ToList(),
            Contacts: partner.Contacts.Select(c => c.ToDto()).ToList()
        );

        return this;
    }

    public UpdatePartnerCommandBuilder TogglingStatus()
    {
        _command = _command with { IsActive = !_command.IsActive };
        return this;
    }

    public UpdatePartnerCommandBuilder WithDisplayName(string name)
    {
        _command = _command with { DisplayName = name };
        return this;
    }

    public UpdatePartnerCommandBuilder WithRoles(params long[] ids)
    {
        _command = _command with { RoleIds = ids };
        return this;
    }

    public UpdatePartnerCommand Build() => _command;
}
