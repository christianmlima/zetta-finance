using MediatR;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Accounts.Commands.DeleteAccount;

internal sealed class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Result>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account is null)
            return Result.Failure(Error.NotFound("Account"));

        if (account.UserId != request.UserId)
            return Result.Failure(Error.Unauthorized());

        _accountRepository.Remove(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
