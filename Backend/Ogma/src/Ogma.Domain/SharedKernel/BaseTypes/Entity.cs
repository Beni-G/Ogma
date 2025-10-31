using Ogma.Domain.SharedKernel.Events;

namespace Ogma.Domain.SharedKernel.BaseTypes;
public abstract class Entity<TKey> : IEquatable<Entity<TKey>>
{
    public TKey Id { get; protected set; }

    public DateTime CreatedAt { get; protected set; }
    public DateTime UpdatedAt { get; protected set; }
    public int Version { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected Entity()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Version = 1;
    }

    protected Entity(TKey id) : this()
    {
        Id = id;
    }

    public void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
        Version++;
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent == null)
        {
            throw new ArgumentNullException(nameof(domainEvent));
        }

        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public override bool Equals(object obj) => Equals(obj as Entity<TKey>);

    public bool Equals(Entity<TKey>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        // Use reference equality for new entities (Id is default, e.g., 0 for long)
        if (EqualityComparer<TKey>.Default.Equals(Id, default(TKey)) &&
            EqualityComparer<TKey>.Default.Equals(other.Id, default(TKey)))
        {
            return ReferenceEquals(this, other);
        }

        return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TKey>.Default.GetHashCode(Id);
    }


}
