using Zetta.Domain.Aggregates.Transactions;
using Zetta.Domain.Enums;

namespace Zetta.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<(List<Transaction> Items, int TotalCount)> GetPagedAsync(
        Guid userId,
        int page,
        int pageSize,
        DateOnly? from = null,
        DateOnly? to = null,
        TransactionType? type = null,
        Guid? categoryId = null,
        Guid? accountId = null,
        CancellationToken cancellationToken = default);

    void Add(Transaction transaction);
    void Update(Transaction transaction);
    void Remove(Transaction transaction);
}
