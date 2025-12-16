using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.SharedKernel.ValueObjects;

public sealed class Period : ValueObject
{
    public DateTime Start { get; }
    public DateTime? End { get; }

    public Period(DateTime start, DateTime? end = null)
    {
        DateTime utcStart = start.ToUniversalTime();
        DateTime? utcEnd = end?.ToUniversalTime();

        if (utcStart == DateTime.MinValue)
        {
            throw new ArgumentException("Start date cannot be DateTime.MinValue.", nameof(start));
        }

        if (utcEnd < utcStart)
        {
            throw new ArgumentException("End date cannot be earlier than start date.", nameof(end));
        }
        Start = utcStart;
        End = utcEnd;
    }

    public bool Contains(DateTime date) => date >= Start && (End == null || date <= End);
    public bool OverlapsWith(Period other) =>
        other.Start <= (End ?? DateTime.MaxValue) &&
        Start <= (other.End ?? DateTime.MaxValue);

    public bool IsOngoing => End == null;

    public TimeSpan? Duration => End.HasValue
        ? End.Value - Start
        : null;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End ?? DateTime.MaxValue;
    }

    public override string ToString() =>
        End is null
            ? $"From {Start:yyyy-MM-dd} onwards"
            : $"From {Start:yyyy-MM-dd} to {End:yyyy-MM-dd}";
}

