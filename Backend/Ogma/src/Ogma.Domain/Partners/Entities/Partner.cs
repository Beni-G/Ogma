using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.Extentions;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Partners.Entities;

public class Partner : AggregateRoot<long>
{
    private List<long> _roleIds = new();
    private List<PartnerIdentifier> _identifiers = new();
    private List<PartnerBankAccount> _bankAccounts = new();
    private List<PartnerContact> _contacts = new();

    public PersonName? IndividualName { get; private set; }
    public string? CompanyName { get; private set; }
    public bool IsNaturalPerson { get; private set; } = false;
    public bool IsActive { get; private set; }
    public string? DisplayName { get; private set; }
    public Address? HQAddress { get; private set; }
    public IReadOnlyCollection<long> RoleIds => _roleIds.AsReadOnly();
    public IReadOnlyCollection<PartnerIdentifier> Identifiers => _identifiers.AsReadOnly();
    public IReadOnlyCollection<PartnerBankAccount> BankAccounts => _bankAccounts.AsReadOnly();
    public IReadOnlyCollection<PartnerContact> Contacts => _contacts.AsReadOnly();

    /// <summary>
    /// Creates a new individual partner with the specified name, primary identifier, and partner role.
    /// </summary>
    /// <param name="individualName"></param>
    /// <param name="primaryIdentifier"></param>
    /// <param name="partnerRoleId"></param>
    /// <param name="hqAddress"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static Partner CreateIndividual(PersonName individualName, PartnerIdentifier primaryIdentifier, long partnerRoleId, Address? hqAddress = null)
    {
        if (individualName == null)
        {
            throw new ArgumentNullException("Individual name cannot be null.", nameof(individualName));
        }
        if (primaryIdentifier == null)
        {
            throw new ArgumentNullException("Primary identifier cannot be null.", nameof(primaryIdentifier));
        }
        if (partnerRoleId <= 0)
        {
            throw new ArgumentException("Partner role ID must be a positive number.", nameof(partnerRoleId));
        }
        var partner = new Partner
        {
            IndividualName = individualName,
            CompanyName = null,
            DisplayName = individualName.FullName,
            IsNaturalPerson = true,
            IsActive = true
        };
        primaryIdentifier.MarkAsPrimary();
        var identifiers = new List<PartnerIdentifier> { primaryIdentifier };
        partner.ReplaceIdentifiers(identifiers);
        var roleIds = new List<long> { partnerRoleId };
        partner.ReplaceRoles(roleIds);
        partner.HQAddress = hqAddress;
        return partner;
    }

    /// <summary>
    /// Creates a new legal entity partner with the specified company name, primary identifier, and partner role.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="primaryIdentifier"></param>
    /// <param name="partnerRoleId"></param>
    /// <param name="hqAddress"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static Partner CreateLegalEntity(string name, PartnerIdentifier primaryIdentifier, long partnerRoleId, Address? hqAddress = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }
        if (primaryIdentifier == null)
        {
            throw new ArgumentNullException("Primary identifier cannot be null.", nameof(primaryIdentifier));
        }
        if (partnerRoleId <= 0)
        {
            throw new ArgumentException("Partner role ID must be a positive number.", nameof(partnerRoleId));
        }
        var partner = new Partner
        {
            CompanyName = name,
            DisplayName = name,
            IsNaturalPerson = false,
            IsActive = true
        };
        primaryIdentifier.MarkAsPrimary();
        var identifiers = new List<PartnerIdentifier> { primaryIdentifier };
        partner.ReplaceIdentifiers(identifiers);
        var roleIds = new List<long> { partnerRoleId };
        partner.ReplaceRoles(roleIds);
        partner.HQAddress = hqAddress;
        return partner;
    }

    /// <summary>
    /// Reconstructs a partner from persisted data, ensuring that all invariants are maintained. This method is intended for use by the repository when rehydrating
    /// </summary>
    /// <param name="id"></param>
    /// <param name="individualName"></param>
    /// <param name="companyName"></param>
    /// <param name="isNaturalPerson"></param>
    /// <param name="isActive"></param>
    /// <param name="displayName"></param>
    /// <param name="hqAddress"></param>
    /// <param name="identifiers"></param>
    /// <param name="roleIds"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="contacts"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Partner Reconstitute(
        long id,
        PersonName? individualName,
        string? companyName,
        bool isNaturalPerson,
        bool isActive,
        string? displayName,
        Address? hqAddress,
        IReadOnlyCollection<PartnerIdentifier> identifiers,
        IReadOnlyCollection<long> roleIds,
        IReadOnlyCollection<PartnerBankAccount> bankAccounts,
        IReadOnlyCollection<PartnerContact> contacts)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Id must be a positive non-zero value.", nameof(id));
        }

        ArgumentNullException.ThrowIfNull(identifiers);
        ArgumentNullException.ThrowIfNull(roleIds);
        ArgumentNullException.ThrowIfNull(bankAccounts);
        ArgumentNullException.ThrowIfNull(contacts);

        EnforceLegalStatusInvariant(individualName, companyName, isNaturalPerson);
        EnsureIdentifiersAndRolesInvariant(identifiers, roleIds);

        var partner = new Partner
        {
            Id = id,
            IndividualName = individualName,
            CompanyName = companyName,
            IsNaturalPerson = isNaturalPerson,
            IsActive = isActive,
            DisplayName = displayName,
            HQAddress = hqAddress
        };

        partner.ReplaceIdentifiers(identifiers);
        partner.ReplaceRoles(roleIds);
        partner.ReplaceBankAccounts(bankAccounts);
        partner.ReplaceContacts(contacts);

        return partner;
    }

    /// <summary>
    /// Updates the partner's information with the specified values, ensuring that all invariants are maintained. 
    /// This method is intended for use by application services when modifying
    /// </summary>
    /// <param name="individualName"></param>
    /// <param name="companyName"></param>
    /// <param name="isNaturalPerson"></param>
    /// <param name="isActive"></param>
    /// <param name="displayName"></param>
    /// <param name="hqAddress"></param>
    /// <param name="identifiers"></param>
    /// <param name="roleIds"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="contacts"></param>
    public void Update(
        PersonName? individualName,
        string? companyName,
        bool isNaturalPerson,
        bool isActive,
        string? displayName,
        Address? hqAddress,
        IReadOnlyCollection<PartnerIdentifier> identifiers,
        IReadOnlyCollection<long> roleIds,
        IReadOnlyCollection<PartnerBankAccount> bankAccounts,
        IReadOnlyCollection<PartnerContact> contacts)
    {
        ArgumentNullException.ThrowIfNull(identifiers);
        ArgumentNullException.ThrowIfNull(roleIds);
        ArgumentNullException.ThrowIfNull(bankAccounts);
        ArgumentNullException.ThrowIfNull(contacts);

        EnforceLegalStatusInvariant(individualName, companyName, isNaturalPerson);
        EnsureIdentifiersAndRolesInvariant(identifiers, roleIds);

        IndividualName = individualName;
        CompanyName = companyName?.Trim();
        IsNaturalPerson = isNaturalPerson;

        IsActive = isActive;

        if (displayName != null)
        {
            UpdateDisplayName(displayName);
        }

        if (hqAddress! != null!)
        {
            UpdateHQAddress(hqAddress);
        }

        ReplaceIdentifiers(identifiers);
        ReplaceRoles(roleIds);
        ReplaceBankAccounts(bankAccounts);
        ReplaceContacts(contacts);

    }

    /// <summary>
    /// Ensures that the provided collections of identifiers and roles are not empty, as a partner must have at least one identifier and one role to be valid.
    /// </summary>
    /// <param name="identifiers"></param>
    /// <param name="roleIds"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private static void EnsureIdentifiersAndRolesInvariant(IReadOnlyCollection<PartnerIdentifier> identifiers, IReadOnlyCollection<long> roleIds)
    {
        if (!identifiers.Any())
        {
            throw new InvalidOperationException("Partner must have at least one identifier");
        }

        if (!roleIds.Any())
        {
            throw new InvalidOperationException("Partner must have at least one role");
        }
    }

    /// <summary>
    /// Validates that exactly one of an individual or company name is provided and that the legal status flag matches
    /// the provided name.
    /// </summary>
    /// <param name="individualName">The name of the individual, or null if the entity is not a natural person.</param>
    /// <param name="companyName">The name of the company, or null or whitespace if the entity is a natural person.</param>
    /// <param name="isNaturalPerson">A value indicating whether the entity is a natural person. Must be <see langword="true"/> if an individual name
    /// is provided; otherwise, <see langword="false"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if both or neither of the names are provided, or if the value of <paramref name="isNaturalPerson"/> does
    /// not match the provided name.</exception>
    private static void EnforceLegalStatusInvariant(PersonName? individualName, string? companyName, bool isNaturalPerson)
    {
        var hasIndividual = individualName != null;
        var hasCompany = !string.IsNullOrWhiteSpace(companyName);

        if (hasIndividual == hasCompany)
        {
            throw new InvalidOperationException(
                hasIndividual
                    ? "Both IndividualName and CompanyName are provided — only one is allowed"
                    : "Neither IndividualName nor CompanyName is provided — exactly one is required");
        }

        if (hasIndividual != isNaturalPerson)
        {
            throw new InvalidOperationException(
                "IsNaturalPerson flag does not match the provided name: " +
                (hasIndividual
                    ? "IndividualName was provided but IsNaturalPerson = false"
                    : "CompanyName was provided but IsNaturalPerson = true"));
        }
    }

    /// <summary>
    /// Returns the full name of the partner.
    /// </summary>
    public string FullName => IsNaturalPerson
        ? IndividualName!.FullName.Trim()
        : CompanyName!;

    /// <summary>
    /// Returns the full name of the partner, including the display name if it exists.
    /// </summary>
    public string FullNameWithDisplay => string.IsNullOrWhiteSpace(DisplayName)
        ? FullName
        : $"{FullName} ({DisplayName})";

    /// <summary>
    /// Returns true if the partner is a legal entity.
    /// </summary>
    public bool IsLegalEntity => !IsNaturalPerson;

    /// <summary>
    /// Replaces the partner's identifiers.
    /// </summary>
    /// <param name="identifiers"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void ReplaceIdentifiers(IEnumerable<PartnerIdentifier> identifiers)
    {
        _identifiers.Clear();
        foreach (var id in identifiers ?? throw new ArgumentNullException(nameof(identifiers)))
        {
            if (id.IsPrimary && _identifiers.Any(x => x.IsPrimary))
            {
                _identifiers.Single(x => x.IsPrimary).UnmarkAsPrimary();
            }

            if (!_identifiers.Any(x => x.IsPrimary))
            {
                id.MarkAsPrimary();
            }

            _identifiers.Add(id);
        }
    }

    /// <summary>
    /// Replaces the partner's role IDs.
    /// </summary>
    /// <param name="roleIds"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void ReplaceRoles(IEnumerable<long> roleIds) =>
        _roleIds.ReplaceWith(roleIds ?? throw new ArgumentNullException(nameof(roleIds)));

    /// <summary>
    /// Replaces the partner's bank accounts.
    /// </summary>
    /// <param name="accounts"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void ReplaceBankAccounts(IEnumerable<PartnerBankAccount> accounts) =>
        _bankAccounts.ReplaceWith(accounts ?? throw new ArgumentNullException());

    /// <summary>
    /// Replaces the partner's contacts.
    /// </summary>
    /// <param name="contacts"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void ReplaceContacts(IEnumerable<PartnerContact> contacts) =>
        _contacts.ReplaceWith(contacts ?? throw new ArgumentNullException());

    /// <summary>
    /// Updates the headquarters address to the specified value.
    /// </summary>
    /// <param name="newAddress">The new address to assign as the headquarters address. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="newAddress"/> is null.</exception>
    private void UpdateHQAddress(Address newAddress)
    {
        if (newAddress == null)
        {
            throw new ArgumentNullException(nameof(newAddress));
        }
        HQAddress = newAddress;
    }

    /// <summary>
    /// Updates the display name of the partner.
    /// </summary>
    /// <param name="newDisplayName"></param>
    /// <exception cref="ArgumentException"></exception>
    private void UpdateDisplayName(string newDisplayName)
    {
        if (string.IsNullOrWhiteSpace(newDisplayName))
        {
            throw new ArgumentException("Display name cannot be null or empty.", nameof(newDisplayName));
        }
        DisplayName = newDisplayName;
    }
}
