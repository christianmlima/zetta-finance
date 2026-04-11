using Zetta.Domain.Enums;
using Zetta.Domain.Events;
using Zetta.Domain.ValueObjects;
using Zetta.SharedKernel.Primitives;
using Zetta.SharedKernel.Results;

namespace Zetta.Domain.Aggregates.Accounts;

public sealed class Account : AggregateRoot
{
    private Account(Guid id, Guid userId, string name, AccountType type, Money openingBalance, DateOnly openingDate)
        : base(id)
    {
        UserId = userId;
        Name = name;
        Type = type;
        OpeningBalance = openingBalance;
        OpeningDate = openingDate;
    }

    private Account() { }

    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public AccountType Type { get; private set; }
    public Money OpeningBalance { get; private set; } = null!;
    public DateOnly OpeningDate { get; private set; }

    public static Result<Account> Create(
        Guid userId,
        string name,
        AccountType type,
        decimal openingBalance = 0,
        DateOnly? openingDate = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Account>(Error.Validation("Account name cannot be empty."));

        var moneyResult = Money.Create(openingBalance);
        if (moneyResult.IsFailure)
            return Result.Failure<Account>(moneyResult.Error);

        var account = new Account(Guid.NewGuid(), userId, name.Trim(), type, moneyResult.Value, openingDate ?? DateOnly.FromDateTime(DateTime.UtcNow));
        account.RaiseDomainEvent(new AccountCreatedEvent(account.Id, userId));

        return account;
    }

    public void Update(string name, AccountType type)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();

        Type = type;
        SetUpdatedAt();
    }
}
