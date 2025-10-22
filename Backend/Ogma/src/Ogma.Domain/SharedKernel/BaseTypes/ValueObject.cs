namespace Ogma.Domain.SharedKernel.BaseTypes;
public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public bool Equals(ValueObject other)
    {
        if (other == null || GetType() != other.GetType())
        {
            return false;
        }
            
        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
            
        return Equals((ValueObject)obj);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(17, (current, obj) =>
                current * 23 + (obj?.GetHashCode() ?? 0));
    }

    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (left is null && right is null)
        {
            return true;
        }
        if (left is null || right is null)
        {
            return false;
        }
        return left.Equals(right);
    }

    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !(left == right);
    }

    // Optional: Deep copy for immutability (if needed)
    public ValueObject Copy()
    {
        return MemberwiseClone() as ValueObject;
    }
}

