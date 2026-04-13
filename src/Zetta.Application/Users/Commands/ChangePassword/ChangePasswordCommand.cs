using MediatR;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword) : IRequest<Result>;
