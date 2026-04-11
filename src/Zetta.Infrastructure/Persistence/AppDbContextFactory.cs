using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Zetta.Infrastructure.Persistence;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Database=zetta;Username=postgres;Password=postgres")
            .Options;

        var publisher = new ServiceCollection()
            .AddLogging()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AppDbContextFactory).Assembly))
            .BuildServiceProvider()
            .GetRequiredService<MediatR.IPublisher>();

        return new AppDbContext(options, publisher);
    }
}
