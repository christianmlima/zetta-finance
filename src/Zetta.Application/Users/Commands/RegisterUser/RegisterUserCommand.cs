using MediatR;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Name,
    string Email,
    string Password) : IRequest<Result<RegisterUserResponse>>;

public sealed record RegisterUserResponse(Guid Id, string Name, string Email);
