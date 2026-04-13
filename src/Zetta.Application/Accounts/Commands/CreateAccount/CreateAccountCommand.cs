using MediatR;
using Zetta.Domain.Enums;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Accounts.Commands.CreateAccount;

public sealed record CreateAccountCommand(
    Guid UserId,
    string Name,
    AccountType Type,
    decimal OpeningBalance = 0,
    DateOnly? OpeningDate = null) : IRequest<Result<CreateAccountResponse>>;

public sealed record CreateAccountResponse(Guid Id, string Name, AccountType Type, decimal OpeningBalance, DateOnly OpeningDate);
