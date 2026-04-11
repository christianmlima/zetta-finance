using Zetta.SharedKernel.Primitives;

namespace Zetta.Domain.Events;

public sealed record UserCreatedEvent(Guid UserId, string Email) : DomainEvent;
