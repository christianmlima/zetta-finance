using MediatR;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Accounts.Queries.GetAccounts;

internal sealed class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, Result<List<AccountResponse>>>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountsQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<List<AccountResponse>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var response = accounts.Select(a => new AccountResponse(
            a.Id, a.Name, a.Type, a.OpeningBalance.Amount, a.OpeningBalance.Currency, a.OpeningDate, a.CreatedAt))
            .ToList();

        return response;
    }
}
