using MediatR;
using Zetta.Application.Accounts.Queries.GetAccounts;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Accounts.Queries.GetAccountById;

public sealed record GetAccountByIdQuery(Guid AccountId, Guid UserId) : IRequest<Result<AccountResponse>>;
