using MediatR;
using Zetta.Application.Common.Interfaces;
using Zetta.Domain.Interfaces;
using Zetta.Domain.ValueObjects;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Users.Commands.Login;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return Result.Failure<LoginResponse>(Error.Unauthorized());

        var user = await _userRepository.GetByEmailAsync(emailResult.Value, cancellationToken);
        if (user is null)
            return Result.Failure<LoginResponse>(Error.Unauthorized());

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Failure<LoginResponse>(Error.Unauthorized());

        var token = _jwtProvider.Generate(user);
        return new LoginResponse(token, user.Id, user.Name, user.Email.Value);
    }
}
