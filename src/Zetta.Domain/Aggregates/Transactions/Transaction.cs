using Zetta.Domain.Enums;
using Zetta.Domain.Events;
using Zetta.Domain.ValueObjects;
using Zetta.SharedKernel.Primitives;
using Zetta.SharedKernel.Results;

namespace Zetta.Domain.Aggregates.Transactions;

public sealed class Transaction : AggregateRoot
{
    private Transaction(
        Guid id,
        Guid userId,
        Guid accountId,
        Guid? categoryId,
        TransactionType type,
        Money amount,
        DateOnly date,
        string description,
        Guid? transferTargetAccountId)
        : base(id)
    {
        UserId = userId;
        AccountId = accountId;
        CategoryId = categoryId;
        Type = type;
        Amount = amount;
        Date = date;
        Description = description;
        TransferTargetAccountId = transferTargetAccountId;
    }

    private Transaction() { }

    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid? CategoryId { get; private set; }
    public TransactionType Type { get; private set; }
    public Money Amount { get; private set; } = null!;
    public DateOnly Date { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public Guid? TransferTargetAccountId { get; private set; }

    public static Result<Transaction> Create(
        Guid userId,
        Guid accountId,
        TransactionType type,
        decimal amount,
        DateOnly date,
        string description,
        Guid? categoryId = null,
        Guid? transferTargetAccountId = null)
    {
        if (amount <= 0)
            return Result.Failure<Transaction>(Error.Validation("Amount must be greater than zero."));

        if (type == TransactionType.Transfer && transferTargetAccountId is null)
            return Result.Failure<Transaction>(Error.Validation("Transfer requires a target account."));

        if (type == TransactionType.Transfer && transferTargetAccountId == accountId)
            return Result.Failure<Transaction>(Error.Validation("Transfer source and target accounts must be different."));

        if (type != TransactionType.Transfer && categoryId is null)
            return Result.Failure<Transaction>(Error.Validation("Category is required for income and expense transactions."));

        var transaction = new Transaction(
            Guid.NewGuid(),
            userId,
            accountId,
            categoryId,
            type,
            Money.Of(amount),
            date,
            description.Trim(),
            transferTargetAccountId);

        transaction.RaiseDomainEvent(new TransactionCreatedEvent(transaction.Id, userId, accountId, amount, type));

        return transaction;
    }

    public void Cancel()
    {
        SoftDelete();
        RaiseDomainEvent(new TransactionDeletedEvent(Id, UserId, AccountId, Amount.Amount, Type));
    }
}
