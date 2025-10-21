namespace Ogma.Domain.SharedKernel.Events;
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
