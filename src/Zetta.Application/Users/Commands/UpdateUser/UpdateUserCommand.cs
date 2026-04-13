using MediatR;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(Guid UserId, string Name) : IRequest<Result>;
