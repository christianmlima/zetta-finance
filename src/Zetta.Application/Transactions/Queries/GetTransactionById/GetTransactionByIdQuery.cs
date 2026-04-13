using MediatR;
using Zetta.Application.Transactions.Queries.GetTransactions;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Transactions.Queries.GetTransactionById;

public sealed record GetTransactionByIdQuery(Guid TransactionId, Guid UserId) : IRequest<Result<TransactionResponse>>;
