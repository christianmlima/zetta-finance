namespace Zetta.SharedKernel.Primitives;

public abstract class AggregateRoot : BaseEntity
{
    private readonly List<DomainEvent> _domainEvents = [];

    protected AggregateRoot(Guid id) : base(id) { }
    
    protected AggregateRoot() { }

    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(DomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
