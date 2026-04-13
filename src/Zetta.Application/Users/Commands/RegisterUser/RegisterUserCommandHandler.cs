using MediatR;
using Zetta.Application.Common.Interfaces;
using Zetta.Domain.Aggregates.Users;
using Zetta.Domain.Interfaces;
using Zetta.Domain.ValueObjects;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Users.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return Result.Failure<RegisterUserResponse>(emailResult.Error);

        var exists = await _userRepository.ExistsByEmailAsync(emailResult.Value, cancellationToken);
        if (exists)
            return Result.Failure<RegisterUserResponse>(Error.Conflict("User"));

        var passwordHash = _passwordHasher.Hash(request.Password);

        var userResult = User.Create(request.Name, emailResult.Value, passwordHash);
        if (userResult.IsFailure)
            return Result.Failure<RegisterUserResponse>(userResult.Error);

        _userRepository.Add(userResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var user = userResult.Value;
        return new RegisterUserResponse(user.Id, user.Name, user.Email.Value);
    }
}
