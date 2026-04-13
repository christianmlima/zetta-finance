using MediatR;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserResponse>>;

public sealed record UserResponse(Guid Id, string Name, string Email, DateTime CreatedAt);
