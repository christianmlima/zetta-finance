using MediatR;
using Zetta.Domain.Enums;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Transactions.Commands.CreateTransaction;

public sealed record CreateTransactionCommand(
    Guid UserId,
    Guid AccountId,
    TransactionType Type,
    decimal Amount,
    DateOnly Date,
    string Description,
    Guid? CategoryId = null,
    Guid? TransferTargetAccountId = null) : IRequest<Result<CreateTransactionResponse>>;

public sealed record CreateTransactionResponse(Guid Id, TransactionType Type, decimal Amount, DateOnly Date, string Description);
