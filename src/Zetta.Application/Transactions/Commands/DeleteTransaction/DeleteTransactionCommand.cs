using MediatR;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Transactions.Commands.DeleteTransaction;

public sealed record DeleteTransactionCommand(Guid TransactionId, Guid UserId) : IRequest<Result>;
