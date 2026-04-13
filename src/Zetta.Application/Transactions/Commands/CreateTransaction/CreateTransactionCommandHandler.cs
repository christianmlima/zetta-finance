using MediatR;
using Zetta.Domain.Aggregates.Transactions;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Transactions.Commands.CreateTransaction;

internal sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<CreateTransactionResponse>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionCommandHandler(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateTransactionResponse>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        if (account is null)
            return Result.Failure<CreateTransactionResponse>(Error.NotFound("Account"));

        if (account.UserId != request.UserId)
            return Result.Failure<CreateTransactionResponse>(Error.Unauthorized());

        var result = Transaction.Create(
            request.UserId,
            request.AccountId,
            request.Type,
            request.Amount,
            request.Date,
            request.Description,
            request.CategoryId,
            request.TransferTargetAccountId);

        if (result.IsFailure)
            return Result.Failure<CreateTransactionResponse>(result.Error);

        _transactionRepository.Add(result.Value);

        if (request.Type == Domain.Enums.TransactionType.Transfer)
        {
            var targetAccount = await _accountRepository.GetByIdAsync(request.TransferTargetAccountId!.Value, cancellationToken);
            if (targetAccount is null)
                return Result.Failure<CreateTransactionResponse>(Error.NotFound("Target account"));

            if (targetAccount.UserId != request.UserId)
                return Result.Failure<CreateTransactionResponse>(Error.Unauthorized());

            var creditResult = Transaction.Create(
                request.UserId,
                request.TransferTargetAccountId.Value,
                Domain.Enums.TransactionType.Transfer,
                request.Amount,
                request.Date,
                request.Description,
                categoryId: null,
                transferTargetAccountId: request.AccountId);

            if (creditResult.IsFailure)
                return Result.Failure<CreateTransactionResponse>(creditResult.Error);

            _transactionRepository.Add(creditResult.Value);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var t = result.Value;
        return new CreateTransactionResponse(t.Id, t.Type, t.Amount.Amount, t.Date, t.Description);
    }
}
