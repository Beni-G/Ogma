using Ogma.Application.Partners.Dtos;
using Ogma.Application.SharedKernel.Extensions;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.Partners.Extensions;

public static class PartnersMappingExtensions
{
    #region ToDto Methods

    public static PartnerRoleTypeDto ToDto(this PartnerRoleType entity) => 
        new PartnerRoleTypeDto(entity.Id, entity.Code, entity.Name, entity.Color);

    public static PartnerBankAccountDto ToDto(this PartnerBankAccount partnerBankAccount) =>
        new PartnerBankAccountDto(partnerBankAccount.BankAccount.ToDto(), partnerBankAccount.IsDefault);

    public static PartnerContactDto ToDto(this PartnerContact partnerContact)
    {
        return new PartnerContactDto(partnerContact.Name.ToDto(), 
            partnerContact.Email?.Value,
            partnerContact.Phone,
            partnerContact.Mobile,
            partnerContact.Title,
            partnerContact.JobTitle,
            partnerContact.IsPrimary);
    }

    public static PartnerIdentifierDto ToDto(this PartnerIdentifier partnerIdentifier) =>
        new PartnerIdentifierDto(partnerIdentifier.Type, partnerIdentifier.Value, partnerIdentifier.ValidityPeriod?.ToDto(), partnerIdentifier.IsPrimary);

    public static PartnerDto ToDto(this Partner entity) 
    { 
        return new PartnerDto(
            entity.Id,
            entity.IndividualName?.ToDto(),
            entity.CompanyName,
            entity.IsNaturalPerson,
            entity.IsActive,
            entity.DisplayName,
            entity.HQAddress?.ToDto(),
            entity.Roles.Select(r => r.ToDto()).ToList(),
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

    // Partner?

    #endregion
}
