using Ogma.Application.Partners.Dtos;
using Ogma.Application.SharedKernel.Extensions;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.Partners.Extensions;

public static class PartnersMappingExtensions
{
    #region ToDto Methods

    public static PartnerRoleTypeDto ToDto(this PartnerRoleType partnerRoleType) => 
        new (partnerRoleType.Id, partnerRoleType.Code, partnerRoleType.Name, partnerRoleType.Color);

    public static PartnerBankAccountDto ToDto(this PartnerBankAccount partnerBankAccount) =>
        new PartnerBankAccountDto(partnerBankAccount.Id, partnerBankAccount.BankAccount.ToDto(), partnerBankAccount.IsDefault);

    public static PartnerContactDto ToDto(this PartnerContact partnerContact)
    {
        return new PartnerContactDto(
            partnerContact.Id,
            partnerContact.Name.ToDto(), 
            partnerContact.Email?.Value,
            partnerContact.Phone,
            partnerContact.Mobile,
            partnerContact.Title,
            partnerContact.JobTitle,
            partnerContact.IsPrimary
        );
    }

    public static PartnerIdentifierDto ToDto(this PartnerIdentifier partnerIdentifier) =>
        new PartnerIdentifierDto(partnerIdentifier.Id, partnerIdentifier.Type, partnerIdentifier.Value, partnerIdentifier.ValidityPeriod?.ToDto(), partnerIdentifier.IsPrimary);

    public static PartnerDto ToDto(this Partner entity, IEnumerable<PartnerRoleTypeDto>? roles = null) 
    { 
        return new PartnerDto(
            entity.Id,
            entity.IndividualName?.ToDto(),
            entity.CompanyName,
            entity.IsNaturalPerson,
            entity.IsActive,
            entity.DisplayName,
            entity.HQAddress?.ToDto(),
            roles?.ToList() ?? [],
            entity.Identifiers.Select(i => i.ToDto()).ToList(),
            entity.BankAccounts.Select(b => b.ToDto()).ToList(),
            entity.Contacts.Select(c => c.ToDto()).ToList());
    }

    #endregion

    #region ToDomain Methods

    public static PartnerRoleType ToDomain(this PartnerRoleTypeDto dto) => 
        PartnerRoleType.Create(dto.Code, dto.Name, dto.Color);

    public static PartnerBankAccount ToDomain(this PartnerBankAccountDto dto) =>
        PartnerBankAccount.Create(dto.BankAccount.ToDomain(), dto.IsDefault);

    public static PartnerContact ToDomain(this PartnerContactDto dto) =>
        PartnerContact.Create(dto.Name.ToDomain(),
            new Email(dto.Email!),
            dto.Phone,
            dto.Mobile,
            dto.Title,
            dto.JobTitle,
            dto.IsPrimary);

    public static PartnerIdentifier ToDomain(this PartnerIdentifierDto dto) =>
        PartnerIdentifier.Create(dto.Type, dto.Value, dto.ValidityPeriod?.ToDomain(), dto.IsPrimary);

    #endregion
}
