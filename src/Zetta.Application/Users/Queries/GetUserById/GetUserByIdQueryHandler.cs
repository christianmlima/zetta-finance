using MediatR;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<UserResponse>(Error.NotFound("User"));

        return new UserResponse(user.Id, user.Name, user.Email.Value, user.CreatedAt);
    }
}
