using Microsoft.EntityFrameworkCore;
using Zetta.Domain.Aggregates.Users;
using Zetta.Domain.Interfaces;
using Zetta.Domain.ValueObjects;

namespace Zetta.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
        await context.Users.FirstOrDefaultAsync(u => u.Email.Value == email.Value, cancellationToken);

    public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
        await context.Users.AnyAsync(u => u.Email.Value == email.Value, cancellationToken);

    public void Add(User user) => context.Users.Add(user);

    public void Update(User user) => context.Users.Update(user);
}
