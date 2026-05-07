namespace Ogma.Domain.SharedKernel.BaseTypes;
public abstract class AggregateRoot<TKey> : Entity<TKey>
{
    protected AggregateRoot() : base() { }
    protected AggregateRoot(TKey id, EntityMetadata metadata) : base(id, metadata) { }
}
