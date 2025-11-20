using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Partners.Entities;
public class PartnerContact : Entity<long>
{
    public long PartnerId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Mobile { get; private set; }
    public string? Title { get; private set; }
    public string? JobTitle { get; private set; }
    public bool IsPrimary { get; private set; } = false;

    /// <summary>
    /// Creates a new instance of the PartnerContact class with the specified parameters.
    /// </summary>
    /// <param name="partnerId"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <param name="mobile"></param>
    /// <param name="title"></param>
    /// <param name="jobTitle"></param>
    /// <param name="isPrimary"></param>
    /// <exception cref="ArgumentException"></exception>
    private PartnerContact(
        long partnerId,
        string firstName,
        string lastName,
        Email? email = null,
        string? phone = null,
        string? mobile = null,
        string? title = null,
        string? jobTitle = null,
        bool isPrimary = false)
    {
        if (partnerId <= 0)
        {
            throw new ArgumentException("Partner ID must be a positive number.", nameof(partnerId));
        }
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
        }
        PartnerId = partnerId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Mobile = mobile;
        Title = title;
        JobTitle = jobTitle;
        IsPrimary = isPrimary;
    }

    /// <summary>
    /// Creates a new instance of the PartnerContact class with the specified parameters, including an ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="partnerId"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <param name="mobile"></param>
    /// <param name="title"></param>
    /// <param name="jobTitle"></param>
    /// <param name="isPrimary"></param>
    /// <exception cref="ArgumentException"></exception>
    private PartnerContact(
        long id,
        long partnerId,
        string firstName,
        string lastName,
        Email? email = null,
        string? phone = null,
        string? mobile = null,
        string? title = null,
        string? jobTitle = null,
        bool isPrimary = false) : base(id)
    {
        if (partnerId <= 0)
        {
            throw new ArgumentException("Partner ID must be a positive number.", nameof(partnerId));
        }
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
        }
        PartnerId = partnerId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Mobile = mobile;
        Title = title;
        JobTitle = jobTitle;
        IsPrimary = isPrimary;
    }

    /// <summary>
    /// Creates a new instance of the PartnerContact class with the specified parameters.
    /// </summary>
    /// <param name="partnerId"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <param name="mobile"></param>
    /// <param name="title"></param>
    /// <param name="jobTitle"></param>
    /// <param name="isPrimary"></param>
    /// <returns></returns>
    public static PartnerContact Create(
        long partnerId,
        string firstName,
        string lastName,
        Email? email = null,
        string? phone = null,
        string? mobile = null,
        string? title = null,
        string? jobTitle = null,
        bool isPrimary = false) =>
            new(partnerId, firstName, lastName, email, phone, mobile, title, jobTitle, isPrimary);

    /// <summary>
    /// Reconstitutes an existing PartnerContact instance with the specified parameters.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="partnerId"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <param name="mobile"></param>
    /// <param name="title"></param>
    /// <param name="jobTitle"></param>
    /// <param name="isPrimary"></param>
    /// <returns></returns>
    public static PartnerContact Reconstitute(
        long id,
        long partnerId,
        string firstName,
        string lastName,
        Email? email = null,
        string? phone = null,
        string? mobile = null,
        string? title = null,
        string? jobTitle = null,
        bool isPrimary = false) =>
            new(id, partnerId, firstName, lastName, email, phone, mobile, title, jobTitle, isPrimary);

    /// <summary>
    /// Returns the full name of the contact by combining the first and last names.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
}
