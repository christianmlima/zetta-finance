using MediatR;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Accounts.Commands.DeleteAccount;

public sealed record DeleteAccountCommand(Guid AccountId, Guid UserId) : IRequest<Result>;
