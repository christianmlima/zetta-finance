using Microsoft.EntityFrameworkCore;
using Zetta.Domain.Aggregates.Categories;
using Zetta.Domain.Interfaces;

namespace Zetta.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<List<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await context.Categories
            .Where(c => c.UserId == userId || c.UserId == null)
            .ToListAsync(cancellationToken);

    public void Add(Category category) => context.Categories.Add(category);

    public void Update(Category category) => context.Categories.Update(category);

    public void Remove(Category category) => category.SoftDelete();
}
