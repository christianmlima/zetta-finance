using MediatR;
using Zetta.Domain.Enums;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Transactions.Queries.GetTransactions;

public sealed record GetTransactionsQuery(
    Guid UserId,
    int Page = 1,
    int PageSize = 20,
    DateOnly? From = null,
    DateOnly? To = null,
    TransactionType? Type = null,
    Guid? CategoryId = null,
    Guid? AccountId = null) : IRequest<Result<PagedTransactionsResponse>>;

public sealed record PagedTransactionsResponse(
    List<TransactionResponse> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages);

public sealed record TransactionResponse(
    Guid Id,
    Guid AccountId,
    Guid? CategoryId,
    TransactionType Type,
    decimal Amount,
    string Currency,
    DateOnly Date,
    string Description,
    Guid? TransferTargetAccountId,
    DateTime CreatedAt);
