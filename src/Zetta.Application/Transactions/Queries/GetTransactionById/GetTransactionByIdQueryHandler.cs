using MediatR;
using Zetta.Application.Transactions.Queries.GetTransactions;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Transactions.Queries.GetTransactionById;

internal sealed class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Result<TransactionResponse>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<TransactionResponse>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);
        if (transaction is null)
            return Result.Failure<TransactionResponse>(Error.NotFound("Transaction"));

        if (transaction.UserId != request.UserId)
            return Result.Failure<TransactionResponse>(Error.Unauthorized());

        return new TransactionResponse(
            transaction.Id, transaction.AccountId, transaction.CategoryId, transaction.Type,
            transaction.Amount.Amount, transaction.Amount.Currency,
            transaction.Date, transaction.Description, transaction.TransferTargetAccountId, transaction.CreatedAt);
    }
}
