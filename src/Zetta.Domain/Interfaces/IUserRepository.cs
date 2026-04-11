using Zetta.Domain.Aggregates.Users;
using Zetta.Domain.ValueObjects;

namespace Zetta.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default);
    void Add(User user);
    void Update(User user);
}
