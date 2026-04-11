using MediatR;

namespace Zetta.SharedKernel.Primitives;

public abstract record DomainEvent(Guid Id, DateTime OccurredOn) : INotification
{
    protected DomainEvent() : this(Guid.NewGuid(), DateTime.UtcNow) { }
}
