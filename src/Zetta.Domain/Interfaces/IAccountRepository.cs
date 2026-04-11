using Zetta.Domain.Aggregates.Accounts;

namespace Zetta.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Account>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    void Add(Account account);
    void Update(Account account);
    void Remove(Account account);
}
