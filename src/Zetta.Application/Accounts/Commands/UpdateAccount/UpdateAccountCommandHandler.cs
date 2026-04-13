using MediatR;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Accounts.Commands.UpdateAccount;

internal sealed class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, Result>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account is null)
            return Result.Failure(Error.NotFound("Account"));

        if (account.UserId != request.UserId)
            return Result.Failure(Error.Unauthorized());

        account.Update(request.Name, request.Type);
        _accountRepository.Update(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
