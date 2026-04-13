using MediatR;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Transactions.Queries.GetTransactions;

internal sealed class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, Result<PagedTransactionsResponse>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<PagedTransactionsResponse>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _transactionRepository.GetPagedAsync(
            request.UserId,
            request.Page,
            request.PageSize,
            request.From,
            request.To,
            request.Type,
            request.CategoryId,
            request.AccountId,
            cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var response = items.Select(t => new TransactionResponse(
            t.Id, t.AccountId, t.CategoryId, t.Type,
            t.Amount.Amount, t.Amount.Currency,
            t.Date, t.Description, t.TransferTargetAccountId, t.CreatedAt))
            .ToList();

        return new PagedTransactionsResponse(response, totalCount, request.Page, request.PageSize, totalPages);
    }
}
