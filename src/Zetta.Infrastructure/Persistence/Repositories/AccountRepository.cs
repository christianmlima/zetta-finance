using Microsoft.EntityFrameworkCore;
using Zetta.Domain.Aggregates.Accounts;
using Zetta.Domain.Interfaces;

namespace Zetta.Infrastructure.Persistence.Repositories;

public sealed class AccountRepository(AppDbContext context) : IAccountRepository
{
    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Accounts.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<List<Account>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await context.Accounts.Where(a => a.UserId == userId).ToListAsync(cancellationToken);

    public void Add(Account account) => context.Accounts.Add(account);

    public void Update(Account account) => context.Accounts.Update(account);

    public void Remove(Account account) => account.SoftDelete();
}
