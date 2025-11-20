using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.Extentions;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Partners.Entities;
public class Partner : AggregateRoot<long>
{
    private List<PartnerRole> _roles = new();
    private List<PartnerIdentifier> _identifiers = new();
    private List<PartnerBankAccount> _bankAccounts = new();
    private List<PartnerContact> _contacts = new();

    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? CompanyName { get; private set; }
    public bool IsNaturalPerson { get; private set; } = false;
    public bool IsActive { get; private set; }
    public string? DisplayName { get; private set; }
    public Address? MainAddress { get; private set; }
    public IReadOnlyCollection<PartnerRole> Roles => _roles.AsReadOnly();
    public IReadOnlyCollection<PartnerIdentifier> Identifiers => _identifiers.AsReadOnly();
    public IReadOnlyCollection<PartnerBankAccount> BankAccounts => _bankAccounts.AsReadOnly();
    public IReadOnlyCollection<PartnerContact> Contacts => _contacts.AsReadOnly();

    /// <summary>
    /// Creates a new individual partner.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="primaryIdentifier"></param>
    /// <param name="partnerRole"></param>
    /// <exception cref="ArgumentException"></exception>
    public static Partner CreateIndividual(string firstName, string lastName, PartnerIdentifier primaryIdentifier, PartnerRole partnerRole)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
        }
        var partner = new Partner
        {
            FirstName = firstName,
            LastName = lastName,
            CompanyName = null,
            DisplayName = $"{firstName} {lastName}",
            IsNaturalPerson = true,
            IsActive = true
        };
        primaryIdentifier.MarkAsPrimary();
        var identifiers = new List<PartnerIdentifier> { primaryIdentifier };
        partner.ReplaceIdentifiers(identifiers);
        var roles = new List<PartnerRole> { partnerRole };
        partner.ReplaceRoles(roles);
        return partner;
    }

    /// <summary>
    /// Creates a new legal entity partner.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="primaryIdentifier"></param>
    /// <param name="partnerRole"></param>
    /// <exception cref="ArgumentException"></exception>
    public static Partner CreateLegalEntity(string name, PartnerIdentifier primaryIdentifier, PartnerRole partnerRole)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
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
        var roles = new List<PartnerRole> { partnerRole };
        partner.ReplaceRoles(roles);
        return partner;
    }

    /// <summary>
    /// Reconstitutes a partner from persisted data.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="companyName"></param>
    /// <param name="isNaturalPerson"></param>
    /// <param name="isActive"></param>
    /// <param name="displayName"></param>
    /// <param name="mainAddress"></param>
    /// <param name="identifiers"></param>
    /// <param name="roles"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="contacts"></param>
    public static Partner Reconstitute(
        long id,
        string? firstName,
        string? lastName,
        string? companyName,
        bool isNaturalPerson,
        bool isActive,
        string? displayName,
        Address? mainAddress,
        IEnumerable<PartnerIdentifier> identifiers,
        IEnumerable<PartnerRole> roles,
        IEnumerable<PartnerBankAccount> bankAccounts,
        IEnumerable<PartnerContact> contacts)
    {
        var partner = new Partner
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            CompanyName = companyName,
            IsNaturalPerson = isNaturalPerson,
            IsActive = isActive,
            DisplayName = displayName,
            MainAddress = mainAddress
        };
        partner.ReplaceIdentifiers(identifiers);
        partner.ReplaceRoles(roles);
        partner.ReplaceBankAccounts(bankAccounts);
        partner.ReplaceContacts(contacts);
        return partner;
    }

    /// <summary>
    /// Updates the partner's information.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="companyName"></param>
    /// <param name="isNaturalPerson"></param>
    /// <param name="isActive"></param>
    /// <param name="displayName"></param>
    /// <param name="mainAddress"></param>
    /// <param name="identifiers"></param>
    /// <param name="roles"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="contacts"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Update(
        string? firstName,
        string? lastName,
        string? companyName,
        bool isNaturalPerson,
        bool isActive,
        string? displayName,
        Address? mainAddress,
        IEnumerable<PartnerIdentifier> identifiers,
        IEnumerable<PartnerRole> roles,
        IEnumerable<PartnerBankAccount> bankAccounts,
        IEnumerable<PartnerContact> contacts)
    {
        if (isNaturalPerson != IsNaturalPerson)
        {
            if (isNaturalPerson)
            {
                // Becoming individual
                FirstName = firstName ?? throw new InvalidOperationException("FirstName required if the partner is a natural person");
                LastName = lastName ?? throw new InvalidOperationException("LastName required if the partner is a natural person");
                CompanyName = null;
            }
            else
            {
                // Becoming legal entity
                CompanyName = companyName ?? throw new InvalidOperationException("CompanyName required if the partner is a legal entity");
                FirstName = null;
                LastName = null;
            }

            IsNaturalPerson = isNaturalPerson;
        }

        if (IsNaturalPerson)
        {
            if (firstName != FirstName)
            {
                FirstName = firstName?.Trim();
            }

            if (lastName != LastName)
            {
                LastName = lastName?.Trim();
            }
        }
        else
        {
            if (companyName != CompanyName)
            {
                CompanyName = companyName?.Trim();
            }
        }

        if (isActive != IsActive)
        {
            IsActive = isActive;
        }

        if (displayName != null)
        {
            UpdateDisplayName(displayName);
        }

        if (mainAddress != null)
        {
            UpdateMainAddress(mainAddress);
        }

        ReplaceRoles(roles);
        ReplaceIdentifiers(identifiers);
        ReplaceBankAccounts(bankAccounts);
        ReplaceContacts(contacts);
    }

    /// <summary>
    /// Returns the full name of the partner.
    /// </summary>
    public string FullName => IsNaturalPerson
        ? $"{FirstName} {LastName}".Trim()
        : CompanyName!;

    /// <summary>
    /// Returns the full name of the partner, including the display name if it exists.
    /// </summary>
    public string FullNameWithDisplay => string.IsNullOrWhiteSpace(DisplayName)
        ? FullName
        : $"{FullName} ({DisplayName})";

    /// <summary>
    /// Replaces the partner's identifiers.
    /// </summary>
    /// <param name="identifiers"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void ReplaceIdentifiers(IEnumerable<PartnerIdentifier> identifiers)
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
    public void ReplaceRoles(IEnumerable<PartnerRole> roles) =>
        _roles.ReplaceWith(roles ?? throw new ArgumentNullException(nameof(roles)));

    /// <summary>
    /// Replaces the partner's bank accounts.
    /// </summary>
    /// <param name="accounts"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void ReplaceBankAccounts(IEnumerable<PartnerBankAccount> accounts) =>
        _bankAccounts.ReplaceWith(accounts ?? throw new ArgumentNullException());

    /// <summary>
    /// Replaces the partner's contacts.
    /// </summary>
    /// <param name="contacts"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void ReplaceContacts(IEnumerable<PartnerContact> contacts) =>
        _contacts.ReplaceWith(contacts ?? throw new ArgumentNullException());

    /// <summary>
    /// Updates the main address of the partner.
    /// </summary>
    /// <param name="newAddress"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void UpdateMainAddress(Address newAddress)
    {
        if (newAddress == null)
        {
            throw new ArgumentNullException(nameof(newAddress));
        }
        MainAddress = newAddress;
    }

    /// <summary>
    /// Updates the display name of the partner.
    /// </summary>
    /// <param name="newDisplayName"></param>
    /// <exception cref="ArgumentException"></exception>
    public void UpdateDisplayName(string newDisplayName)
    {
        if (string.IsNullOrWhiteSpace(newDisplayName))
        {
            throw new ArgumentException("Display name cannot be null or empty.", nameof(newDisplayName));
        }
        DisplayName = newDisplayName;
    }

    /// <summary>
    /// Returns true if the partner is a legal entity.
    /// </summary>
    public bool IsLegalEntity => !IsNaturalPerson;
}
