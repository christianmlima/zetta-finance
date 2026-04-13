using MediatR;
using Zetta.Domain.Enums;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Accounts.Queries.GetAccounts;

public sealed record GetAccountsQuery(Guid UserId) : IRequest<Result<List<AccountResponse>>>;

public sealed record AccountResponse(
    Guid Id,
    string Name,
    AccountType Type,
    decimal OpeningBalance,
    string Currency,
    DateOnly OpeningDate,
    DateTime CreatedAt);
