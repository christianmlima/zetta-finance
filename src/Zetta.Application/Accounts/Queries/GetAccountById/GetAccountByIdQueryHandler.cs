using MediatR;
using Zetta.Application.Accounts.Queries.GetAccounts;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Accounts.Queries.GetAccountById;

internal sealed class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Result<AccountResponse>>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountByIdQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<AccountResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account is null)
            return Result.Failure<AccountResponse>(Error.NotFound("Account"));

        if (account.UserId != request.UserId)
            return Result.Failure<AccountResponse>(Error.Unauthorized());

        return new AccountResponse(
            account.Id, account.Name, account.Type,
            account.OpeningBalance.Amount, account.OpeningBalance.Currency,
            account.OpeningDate, account.CreatedAt);
    }
}
