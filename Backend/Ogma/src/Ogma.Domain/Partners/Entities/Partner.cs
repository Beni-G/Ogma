using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.Extentions;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Partners.Entities;

public class Partner : AggregateRoot<long>
{
    private List<PartnerRoleType> _roles = new();
    private List<PartnerIdentifier> _identifiers = new();
    private List<PartnerBankAccount> _bankAccounts = new();
    private List<PartnerContact> _contacts = new();

    public PersonName? IndividualName { get; private set; }
    public string? CompanyName { get; private set; }
    public bool IsNaturalPerson { get; private set; } = false;
    public bool IsActive { get; private set; }
    public string? DisplayName { get; private set; }
    public Address? HQAddress { get; private set; }
    public IReadOnlyCollection<PartnerRoleType> Roles => _roles.AsReadOnly();
    public IReadOnlyCollection<PartnerIdentifier> Identifiers => _identifiers.AsReadOnly();
    public IReadOnlyCollection<PartnerBankAccount> BankAccounts => _bankAccounts.AsReadOnly();
    public IReadOnlyCollection<PartnerContact> Contacts => _contacts.AsReadOnly();

    /// <summary>
    /// Creates a new Partner instance representing an individual with the specified name, primary identifier, and
    /// partner role.
    /// </summary>
    /// <param name="individualName">The name of the individual to associate with the Partner. Cannot be null.</param>
    /// <param name="primaryIdentifier">The primary identifier to assign to the Partner. Cannot be null.</param>
    /// <param name="partnerRole">The role to assign to the Partner. Cannot be null.</param>
    /// <returns>A Partner object representing the individual, initialized with the specified name, primary identifier, and role.</returns>
    /// <exception cref="ArgumentNullException">Thrown if individualName, primaryIdentifier, or partnerRole is null.</exception>
    public static Partner CreateIndividual(PersonName individualName, PartnerIdentifier primaryIdentifier, PartnerRoleType partnerRole)
    {
        if (individualName == null)
        {
            throw new ArgumentNullException("Individual name cannot be null.", nameof(individualName));
        }
        if (primaryIdentifier == null)
        {
            throw new ArgumentNullException("Primary identifier cannot be null.", nameof(primaryIdentifier));
        }
        if (partnerRole == null)
        {
            throw new ArgumentNullException("Partner role cannot be null.", nameof(partnerRole));
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
        var roles = new List<PartnerRoleType> { partnerRole };
        partner.ReplaceRoles(roles);
        return partner;
    }

    /// <summary>
    /// Creates a new legal entity partner with the specified name, primary identifier, and partner role.
    /// </summary>
    /// <param name="name">The legal name of the entity to be created. Cannot be null or empty.</param>
    /// <param name="primaryIdentifier">The primary identifier to associate with the legal entity. Cannot be null.</param>
    /// <param name="partnerRole">The partner role to assign to the legal entity. Cannot be null.</param>
    /// <returns>A new Partner instance representing the legal entity with the specified name, primary identifier, and partner
    /// role.</returns>
    /// <exception cref="ArgumentException">Thrown if the name is null, empty, or consists only of white-space characters.</exception>
    /// <exception cref="ArgumentNullException">Thrown if primaryIdentifier or partnerRole is null.</exception>
    public static Partner CreateLegalEntity(string name, PartnerIdentifier primaryIdentifier, PartnerRoleType partnerRole)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }
        if (primaryIdentifier == null)
        {
            throw new ArgumentNullException("Primary identifier cannot be null.", nameof(primaryIdentifier));
        }
        if (partnerRole == null)
        {
            throw new ArgumentNullException("Partner role cannot be null.", nameof(partnerRole));
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
        var roles = new List<PartnerRoleType> { partnerRole };
        partner.ReplaceRoles(roles);
        return partner;
    }

    /// <summary>
    /// Reconstitutes a Partner instance from the specified persisted data, enforcing required invariants for identity,
    /// type, and roles.
    /// </summary>
    /// <remarks>Use this method to restore a Partner from persisted state, such as when loading from a
    /// database or event stream. The method enforces that exactly one of individual or company name is provided, and
    /// that the partner has at least one identifier and one role.</remarks>
    /// <param name="id">The unique identifier of the partner.</param>
    /// <param name="individualName">The personal name of the partner if the partner is a natural person; otherwise, null.</param>
    /// <param name="companyName">The company name of the partner if the partner is a legal entity; otherwise, null.</param>
    /// <param name="isNaturalPerson">Indicates whether the partner is a natural person. Must be <see langword="true"/> if <paramref
    /// name="individualName"/> is provided; otherwise, <see langword="false"/>.</param>
    /// <param name="isActive">Indicates whether the partner is currently active.</param>
    /// <param name="displayName">An optional display name for the partner, used for presentation purposes.</param>
    /// <param name="mainAddress">The main address associated with the partner, or null if not specified.</param>
    /// <param name="identifiers">A collection of identifiers that uniquely identify the partner. Must contain at least one element and cannot be
    /// null.</param>
    /// <param name="roles">A collection of roles assigned to the partner. Must contain at least one element and cannot be null.</param>
    /// <param name="bankAccounts">A collection of bank accounts associated with the partner. Cannot be null but may be empty.</param>
    /// <param name="contacts">A collection of contacts related to the partner. Cannot be null but may be empty.</param>
    /// <returns>A Partner instance reconstructed from the provided data.</returns>
    /// <exception cref="InvalidOperationException">Thrown if both or neither of <paramref name="individualName"/> and <paramref name="companyName"/> are provided;
    /// if <paramref name="isNaturalPerson"/> does not match the provided name; if <paramref name="identifiers"/> or
    /// <paramref name="roles"/> are empty.</exception>
    public static Partner Reconstitute(
        long id,
        PersonName? individualName,
        string? companyName,
        bool isNaturalPerson,
        bool isActive,
        string? displayName,
        Address? mainAddress,
        IReadOnlyCollection<PartnerIdentifier> identifiers,
        IReadOnlyCollection<PartnerRoleType> roles,
        IReadOnlyCollection<PartnerBankAccount> bankAccounts,
        IReadOnlyCollection<PartnerContact> contacts)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Id must be a positive non-zero value.", nameof(id));
        }

        ArgumentNullException.ThrowIfNull(identifiers);
        ArgumentNullException.ThrowIfNull(roles);
        ArgumentNullException.ThrowIfNull(bankAccounts);
        ArgumentNullException.ThrowIfNull(contacts);

        EnforceLegalStatusInvariant(individualName, companyName, isNaturalPerson);
        EnsureIdentifiersAndRolesInvariant(identifiers, roles);

        var partner = new Partner
        {
            Id = id,
            IndividualName = individualName,
            CompanyName = companyName,
            IsNaturalPerson = isNaturalPerson,
            IsActive = isActive,
            DisplayName = displayName,
            HQAddress = mainAddress
        };

        partner.ReplaceIdentifiers(identifiers);
        partner.ReplaceRoles(roles);
        partner.ReplaceBankAccounts(bankAccounts);
        partner.ReplaceContacts(contacts);

        return partner;
    }

    /// <summary>
    /// Updates the partner's core information, including name, status, identifiers, roles, bank accounts, and contacts.
    /// </summary>
    /// <param name="individualName">The personal name of the partner if the partner is a natural person. Required when changing to a natural person;
    /// otherwise, ignored.</param>
    /// <param name="companyName">The company name of the partner if the partner is a legal entity. Required when changing to a legal entity;
    /// otherwise, ignored.</param>
    /// <param name="isNaturalPerson">A value indicating whether the partner is a natural person. If the value differs from the current state, the
    /// partner type is updated accordingly.</param>
    /// <param name="isActive">A value indicating whether the partner is active. If the value differs from the current state, the partner's
    /// active status is updated.</param>
    /// <param name="displayName">The display name for the partner. If not null, updates the partner's display name.</param>
    /// <param name="mainAddress">The main address of the partner. If not null, updates the partner's main address.</param>
    /// <param name="identifiers">A collection of identifiers associated with the partner. Must contain at least one identifier. Cannot be null.</param>
    /// <param name="roles">A collection of roles assigned to the partner. Must contain at least one role. Cannot be null.</param>
    /// <param name="bankAccounts">A collection of bank accounts associated with the partner. Cannot be null.</param>
    /// <param name="contacts">A collection of contacts associated with the partner. Cannot be null.</param>
    /// <exception cref="InvalidOperationException">Thrown if required name information is missing when changing the partner type, or if no identifiers or roles are
    /// provided.</exception>
    public void Update(
        PersonName? individualName,
        string? companyName,
        bool isNaturalPerson,
        bool isActive,
        string? displayName,
        Address? mainAddress,
        IReadOnlyCollection<PartnerIdentifier> identifiers,
        IReadOnlyCollection<PartnerRoleType> roles,
        IReadOnlyCollection<PartnerBankAccount> bankAccounts,
        IReadOnlyCollection<PartnerContact> contacts)
    {
        ArgumentNullException.ThrowIfNull(identifiers);
        ArgumentNullException.ThrowIfNull(roles);
        ArgumentNullException.ThrowIfNull(bankAccounts);
        ArgumentNullException.ThrowIfNull(contacts);

        EnforceLegalStatusInvariant(individualName, companyName, isNaturalPerson);
        EnsureIdentifiersAndRolesInvariant(identifiers, roles);

        IndividualName = individualName;
        CompanyName = companyName?.Trim();
        IsNaturalPerson = isNaturalPerson;

        IsActive = isActive;

        if (displayName != null)
        {
            UpdateDisplayName(displayName);
        }

        if (mainAddress != null)
        {
            UpdateMainAddress(mainAddress);
        }

        ReplaceIdentifiers(identifiers);
        ReplaceRoles(roles);
        ReplaceBankAccounts(bankAccounts);
        ReplaceContacts(contacts);

    }

    /// <summary>
    /// Ensures that the specified collections of partner identifiers and partner roles are not empty.
    /// </summary>
    /// <param name="identifiers">A read-only collection of partner identifiers to validate. The collection must contain at least one element.</param>
    /// <param name="roles">A read-only collection of partner roles to validate. The collection must contain at least one element.</param>
    /// <exception cref="InvalidOperationException">Thrown if the <paramref name="identifiers"/> collection is empty or if the <paramref name="roles"/> collection
    /// is empty.</exception>
    private static void EnsureIdentifiersAndRolesInvariant(IReadOnlyCollection<PartnerIdentifier> identifiers, IReadOnlyCollection<PartnerRoleType> roles)
    {
        if (!identifiers.Any())
        {
            throw new InvalidOperationException("Partner must have at least one identifier");
        }

        if (!roles.Any())
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
    /// Replaces the partner's roles.
    /// </summary>
    /// <param name="roles"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void ReplaceRoles(IEnumerable<PartnerRoleType> roles) =>
        _roles.ReplaceWith(roles ?? throw new ArgumentNullException(nameof(roles)));

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
    /// Updates the main address of the partner.
    /// </summary>
    /// <param name="newAddress"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void UpdateMainAddress(Address newAddress)
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
