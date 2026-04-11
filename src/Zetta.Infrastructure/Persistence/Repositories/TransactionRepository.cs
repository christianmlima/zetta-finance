using Microsoft.EntityFrameworkCore;
using Zetta.Domain.Aggregates.Transactions;
using Zetta.Domain.Enums;
using Zetta.Domain.Interfaces;

namespace Zetta.Infrastructure.Persistence.Repositories;

public sealed class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<(List<Transaction> Items, int TotalCount)> GetPagedAsync(
        Guid userId,
        int page,
        int pageSize,
        DateOnly? from = null,
        DateOnly? to = null,
        TransactionType? type = null,
        Guid? categoryId = null,
        Guid? accountId = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Transactions.Where(t => t.UserId == userId);

        if (from.HasValue)
            query = query.Where(t => t.Date >= from.Value);

        if (to.HasValue)
            query = query.Where(t => t.Date <= to.Value);

        if (type.HasValue)
            query = query.Where(t => t.Type == type.Value);

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        if (accountId.HasValue)
            query = query.Where(t => t.AccountId == accountId.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(t => t.Date)
            .ThenByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public void Add(Transaction transaction) => context.Transactions.Add(transaction);

    public void Update(Transaction transaction) => context.Transactions.Update(transaction);

    public void Remove(Transaction transaction) => transaction.Cancel();
}
