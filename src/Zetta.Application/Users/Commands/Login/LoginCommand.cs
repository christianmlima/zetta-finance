using MediatR;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Users.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;

public sealed record LoginResponse(string Token, Guid UserId, string Name, string Email);
