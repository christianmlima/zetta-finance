using Zetta.SharedKernel.Primitives;

namespace Zetta.Domain.Events;

public sealed record AccountCreatedEvent(Guid AccountId, Guid UserId) : DomainEvent;
