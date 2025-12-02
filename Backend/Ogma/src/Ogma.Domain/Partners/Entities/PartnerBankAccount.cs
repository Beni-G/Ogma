using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Partners.Entities;
public class PartnerBankAccount : Entity<long>
{
    public long PartnerId { get; private set; }
    public BankAccount BankAccount { get; private set; }
    public bool IsDefault { get; private set; }

    /// <summary>
    /// Creates a new instance of the <see cref="PartnerBankAccount"/> class with the specified partner ID, bank account,
    /// </summary>
    /// <param name="partnerId"></param>
    /// <param name="bankAccount"></param>
    /// <param name="isDefault"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    private PartnerBankAccount(long partnerId, BankAccount bankAccount, bool isDefault = false)
    {
        if (partnerId <= 0)
        {
            throw new ArgumentException("Partner ID must be a positive number.", nameof(partnerId));
        }
        if (bankAccount is null)
        {
            throw new ArgumentNullException(nameof(bankAccount));
        }
        PartnerId = partnerId;
        BankAccount = bankAccount;
        IsDefault = isDefault;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PartnerBankAccount"/> class with the specified ID, partner ID, bank account,
    /// </summary>
    /// <param name="id"></param>
    /// <param name="partnerId"></param>
    /// <param name="bankAccount"></param>
    /// <param name="isDefault"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    private PartnerBankAccount(long id, long partnerId, BankAccount bankAccount, bool isDefault = false) : base(id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID must be a positive number.", nameof(id));
        }
        if (partnerId <= 0)
        {
            throw new ArgumentException("Partner ID must be a positive number.", nameof(partnerId));
        }
        if (bankAccount is null)
        {
            throw new ArgumentNullException(nameof(bankAccount));
        }
        PartnerId = partnerId;
        BankAccount = bankAccount;
        IsDefault = isDefault;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PartnerBankAccount"/> class with the specified partner ID, bank account,
    /// </summary>
    /// <param name="partnerId"></param>
    /// <param name="bankAccount"></param>
    /// <param name="isDefault"></param>
    /// <returns></returns>
    public static PartnerBankAccount Create(long partnerId, BankAccount bankAccount, bool isDefault = false) =>
        new(partnerId, bankAccount, isDefault);

    /// <summary>
    /// Reconstitutes an existing instance of the <see cref="PartnerBankAccount"/> class with the specified ID, partner ID, bank account,
    /// </summary>
    /// <param name="id"></param>
    /// <param name="partnerId"></param>
    /// <param name="bankAccount"></param>
    /// <param name="isDefault"></param>
    /// <returns></returns>
    public static PartnerBankAccount Reconstitute(long id, long partnerId, BankAccount bankAccount, bool isDefault = false) =>
        new(id, partnerId, bankAccount, isDefault);

    public void UpdateBankAccount(BankAccount bankAccount, bool isDefault)
    {
        BankAccount = bankAccount ?? throw new ArgumentNullException(nameof(bankAccount));
        IsDefault = isDefault;
    }
}
