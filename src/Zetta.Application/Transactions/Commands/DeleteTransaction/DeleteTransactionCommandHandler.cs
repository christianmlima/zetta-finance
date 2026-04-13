using MediatR;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Transactions.Commands.DeleteTransaction;

internal sealed class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);
        if (transaction is null)
            return Result.Failure(Error.NotFound("Transaction"));

        if (transaction.UserId != request.UserId)
            return Result.Failure(Error.Unauthorized());

        transaction.Cancel();
        _transactionRepository.Update(transaction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
