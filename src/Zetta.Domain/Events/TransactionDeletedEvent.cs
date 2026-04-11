using Zetta.Domain.Enums;
using Zetta.SharedKernel.Primitives;

namespace Zetta.Domain.Events;

public sealed record TransactionDeletedEvent(
    Guid TransactionId,
    Guid UserId,
    Guid AccountId,
    decimal Amount,
    TransactionType Type) : DomainEvent;
