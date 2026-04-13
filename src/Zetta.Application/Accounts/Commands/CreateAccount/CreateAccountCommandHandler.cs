using MediatR;
using Zetta.Domain.Aggregates.Accounts;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Accounts.Commands.CreateAccount;

internal sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<CreateAccountResponse>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateAccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var result = Account.Create(request.UserId, request.Name, request.Type, request.OpeningBalance, request.OpeningDate);
        if (result.IsFailure)
            return Result.Failure<CreateAccountResponse>(result.Error);

        _accountRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var account = result.Value;
        return new CreateAccountResponse(account.Id, account.Name, account.Type, account.OpeningBalance.Amount, account.OpeningDate);
    }
}
