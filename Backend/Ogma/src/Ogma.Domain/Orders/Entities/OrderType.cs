using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.Orders.Entities;

public class OrderType : AggregateRoot<long>
{
    public string Code { get; private set; }
    public string Description { get; private set; }

    private OrderType(string code, string description)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException(nameof(code));
        }

        Code = code;
        Description = description;
    }

    private OrderType(long id, string code, string description)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID must be a positive number.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException(nameof(code));
        }

        Id = id;
        Code = code;
        Description = description;
    }

    /// <summary>
    /// Creates a new OrderType instance.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static OrderType Create(string code, string description) => new(code, description);

    /// <summary>
    /// Reconstitutes an existing OrderType instance.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="code"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static OrderType Reconstitute(long id, string code, string description) => new(id, code, description);

    /// <summary>
    /// Updates the OrderType instance.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="description"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Update(string code, string description)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException(nameof(code));
        }

        Code = code;
        Description = description;
    }
}
