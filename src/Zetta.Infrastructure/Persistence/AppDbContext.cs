using MediatR;
using Microsoft.EntityFrameworkCore;
using Zetta.Domain.Aggregates.Accounts;
using Zetta.Domain.Aggregates.Categories;
using Zetta.Domain.Aggregates.Transactions;
using Zetta.Domain.Aggregates.Users;
using Zetta.Infrastructure.Persistence.Preferences;
using Zetta.SharedKernel.Primitives;

namespace Zetta.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<AccountPreference> AccountPreferences => Set<AccountPreference>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync(cancellationToken);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var aggregates = ChangeTracker
            .Entries<AggregateRoot>()
            .Select(e => e.Entity)
            .Where(a => a.DomainEvents.Count != 0)
            .ToList();

        var events = aggregates.SelectMany(a => a.DomainEvents).ToList();

        aggregates.ForEach(a => a.ClearDomainEvents());

        foreach (var domainEvent in events)
            await publisher.Publish(domainEvent, cancellationToken);
    }
}
