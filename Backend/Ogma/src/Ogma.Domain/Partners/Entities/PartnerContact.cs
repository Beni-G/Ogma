using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Partners.Entities;

public class PartnerContact : Entity<long>
{
    public PersonName Name { get; private set; }
    public Email? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Mobile { get; private set; }
    public string? Title { get; private set; }
    public string? JobTitle { get; private set; }
    public bool IsPrimary { get; private set; } = false;

    /// <summary>
    /// Creates a new instance of the PartnerContact class with the specified parameters.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <param name="mobile"></param>
    /// <param name="title"></param>
    /// <param name="jobTitle"></param>
    /// <param name="isPrimary"></param>
    /// <exception cref="ArgumentException"></exception>
    private PartnerContact(
        PersonName name,
        Email? email = null,
        string? phone = null,
        string? mobile = null,
        string? title = null,
        string? jobTitle = null,
        bool isPrimary = false)
    {
        if (name == null)
        {
            throw new ArgumentNullException("Name cannot be null.", nameof(name));
        }

        Name = name;
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
    /// <param name="name"></param>
    /// <param name="metadata"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <param name="mobile"></param>
    /// <param name="title"></param>
    /// <param name="jobTitle"></param>
    /// <param name="isPrimary"></param>
    /// <exception cref="ArgumentException"></exception>
    private PartnerContact(
        long id,
        PersonName name,
        EntityMetadata metadata,
        Email? email = null,
        string? phone = null,
        string? mobile = null,
        string? title = null,
        string? jobTitle = null,
        bool isPrimary = false) : base(id, metadata)
    {
        if (name == null)
        {
            throw new ArgumentNullException("Name cannot be null.", nameof(name));
        }

        Name = name;
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
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <param name="mobile"></param>
    /// <param name="title"></param>
    /// <param name="jobTitle"></param>
    /// <param name="isPrimary"></param>
    /// <returns></returns>
    public static PartnerContact Create(
        PersonName name,
        Email? email = null,
        string? phone = null,
        string? mobile = null,
        string? title = null,
        string? jobTitle = null,
        bool isPrimary = false) 
        => new(name, email, phone, mobile, title, jobTitle, isPrimary);

    /// <summary>
    /// Reconstitutes an existing PartnerContact instance with the specified parameters.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="metadata"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <param name="mobile"></param>
    /// <param name="title"></param>
    /// <param name="jobTitle"></param>
    /// <param name="isPrimary"></param>
    /// <returns></returns>
    public static PartnerContact Reconstitute(
        long id,
        PersonName name,
        EntityMetadata metadata,
        Email? email = null,
        string? phone = null,
        string? mobile = null,
        string? title = null,
        string? jobTitle = null,
        bool isPrimary = false) 
        => new(id, name, metadata, email, phone, mobile, title, jobTitle, isPrimary);

    public void Update(
        PersonName name,
        Email? email = null,
        string? phone = null,
        string? mobile = null,
        string? title = null,
        string? jobTitle = null,
        bool isPrimary = false)
    {
        UpdateName(name);
        UpdateEmail(email);
        UpdatePhone(phone);
        UpdateMobile(mobile);
        UpdateTitle(title);
        UpdateJobTitle(jobTitle);
        if (isPrimary)
        {
            MarkAsPrimary();
        }
        else
        {
            UnmarkAsPrimary();
        }
        Touch();
    }

    private void UpdateName(PersonName name) => Name = name ?? throw new ArgumentNullException("Name cannot be null.", nameof(name));

    private void UpdateEmail(Email? email) => Email = email;

    private void UpdatePhone(string? phone) => Phone = phone;

    private void UpdateMobile(string? mobile) => Mobile = mobile;

    private void UpdateTitle(string? title) => Title = title;

    private void UpdateJobTitle(string? jobTitle) => JobTitle = jobTitle;

    private void MarkAsPrimary() => IsPrimary = true;

    private void UnmarkAsPrimary() => IsPrimary = false;
}
