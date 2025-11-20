using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.SharedKernel.ValueObjects;
public sealed class ValidityPeriod : ValueObject
{
    public DateTime Start { get; }
    public DateTime? End { get; }

    public ValidityPeriod(DateTime start, DateTime? end = null)
    {
        if (end is not null && end < start)
        {
            throw new ArgumentException("End date cannot be earlier than start date.", nameof(end));
        }

        Start = start;
        End = end;
    }

    public bool IsActive(DateTime onDate) => onDate >= Start && (End == null || onDate <= End);

    public bool Overlaps(ValidityPeriod other) =>
        other.Start <= (End ?? DateTime.MaxValue) &&
        Start <= (other.End ?? DateTime.MaxValue);


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End ?? DateTime.MaxValue;
    }

    public override string ToString() => End is null ? $"From {Start:yyyy-MM-dd} onwards" : $"From {Start:yyyy-MM-dd} to {End:yyyy-MM-dd}";
}

