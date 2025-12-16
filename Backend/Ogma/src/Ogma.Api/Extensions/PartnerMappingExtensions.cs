using Ogma.Api.Contracts.Partners;
using Ogma.Application.Partners.Dtos;
using Ogma.Domain.Partners.Entities;

namespace Ogma.Api.Extensions;

public static class PartnerMappingExtensions
{
    public static PartnerRoleTypeResponse ToResponse(this PartnerRoleType partnerRoleType) => 
        new PartnerRoleTypeResponse(partnerRoleType.Id, partnerRoleType.Code, partnerRoleType.Name, partnerRoleType.Color);

    public static PartnerRoleTypeResponse ToResponse(this PartnerRoleTypeDto partnerRoleType) =>
        new PartnerRoleTypeResponse(partnerRoleType.Id, partnerRoleType.Code, partnerRoleType.Name, partnerRoleType.Color);

    public static PartnerBankAccountResponse ToResponse(this PartnerBankAccount partnerBankAccount) =>
        new PartnerBankAccountResponse(partnerBankAccount.BankAccount.ToResponse(), partnerBankAccount.IsDefault);

    public static PartnerBankAccountResponse ToResponse(this PartnerBankAccountDto partnerBankAccount) =>
        new PartnerBankAccountResponse(partnerBankAccount.BankAccount.ToResponse(), partnerBankAccount.IsDefault);

    public static PartnerIdentifierResponse ToResponse(this PartnerIdentifier partnerIdentifier) =>
        new PartnerIdentifierResponse(
            partnerIdentifier.Type,
            partnerIdentifier.Value,
            partnerIdentifier.ValidityPeriod?.ToResponse(),
            partnerIdentifier.IsPrimary);

    public static PartnerIdentifierResponse ToResponse(this PartnerIdentifierDto partnerIdentifier) =>
        new PartnerIdentifierResponse(
            partnerIdentifier.Type,
            partnerIdentifier.Value,
            partnerIdentifier.ValidityPeriod?.ToResponse(),
            partnerIdentifier.IsPrimary);

    public static PartnerContactResponse ToResponse(this PartnerContact partnerContact) =>
        new PartnerContactResponse(
            partnerContact.Name.ToResponse(),
            partnerContact.Email?.Value,
            partnerContact.Phone,
            partnerContact.Mobile,
            partnerContact.Title,
            partnerContact.JobTitle,
            partnerContact.IsPrimary);

    public static PartnerContactResponse ToResponse(this PartnerContactDto partnerContact) =>
        new PartnerContactResponse(
            partnerContact.Name.ToResponse(),
            partnerContact.Email,
            partnerContact.Phone,
            partnerContact.Mobile,
            partnerContact.Title,
            partnerContact.JobTitle,
            partnerContact.IsPrimary);

    public static PartnerResponse ToResponse(this Partner partner) =>
        new PartnerResponse(
            partner.Id,
            partner.IndividualName?.ToResponse(),
            partner.CompanyName,
            partner.IsNaturalPerson,
            partner.IsActive,
            partner.DisplayName,
            partner.HQAddress?.ToResponse(),
            partner.Roles.Select(r => r.ToResponse()).ToList(),
            partner.Identifiers.Select(i => i.ToResponse()).ToList(),
            partner.BankAccounts.Select(b => b.ToResponse()).ToList(),
            partner.Contacts.Select(c => c.ToResponse()).ToList());

    public static PartnerResponse ToResponse(this PartnerDto partner) =>
        new PartnerResponse(
            partner.Id,
            partner.IndividualName?.ToResponse(),
            partner.CompanyName,
            partner.IsNaturalPerson,
            partner.IsActive,
            partner.DisplayName,
            partner.HQAddress?.ToResponse(),
            partner.Roles.Select(r => r.ToResponse()).ToList(),
            partner.Identifiers.Select(i => i.ToResponse()).ToList(),
            partner.BankAccounts.Select(b => b.ToResponse()).ToList(),
            partner.Contacts.Select(c => c.ToResponse()).ToList());

}
