using MediatR;
using Zetta.Domain.Enums;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Accounts.Commands.UpdateAccount;

public sealed record UpdateAccountCommand(
    Guid AccountId,
    Guid UserId,
    string Name,
    AccountType Type) : IRequest<Result>;
