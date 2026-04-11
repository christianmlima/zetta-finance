using Zetta.Domain.Aggregates.Categories;
using Zetta.Domain.Enums;

namespace Zetta.Infrastructure.Persistence.Seeds;

public static class CategorySeed
{
    public static IEnumerable<object> GetCategories()
    {
        var expense = new[]
        {
            (Id: Guid.Parse("10000000-0000-0000-0000-000000000001"), Name: "Housing",       Icon: "home",          Color: "#EF4444"),
            (Id: Guid.Parse("10000000-0000-0000-0000-000000000002"), Name: "Food",          Icon: "utensils",      Color: "#F97316"),
            (Id: Guid.Parse("10000000-0000-0000-0000-000000000003"), Name: "Transport",     Icon: "car",           Color: "#EAB308"),
            (Id: Guid.Parse("10000000-0000-0000-0000-000000000004"), Name: "Health",        Icon: "heart-pulse",   Color: "#22C55E"),
            (Id: Guid.Parse("10000000-0000-0000-0000-000000000005"), Name: "Education",     Icon: "graduation-cap",Color: "#3B82F6"),
            (Id: Guid.Parse("10000000-0000-0000-0000-000000000006"), Name: "Entertainment", Icon: "tv",            Color: "#8B5CF6"),
            (Id: Guid.Parse("10000000-0000-0000-0000-000000000007"), Name: "Clothing",      Icon: "shirt",         Color: "#EC4899"),
            (Id: Guid.Parse("10000000-0000-0000-0000-000000000008"), Name: "Personal Care", Icon: "sparkles",      Color: "#14B8A6"),
            (Id: Guid.Parse("10000000-0000-0000-0000-000000000009"), Name: "Taxes",         Icon: "landmark",      Color: "#6B7280"),
            (Id: Guid.Parse("10000000-0000-0000-0000-000000000010"), Name: "Other",         Icon: "ellipsis",      Color: "#9CA3AF"),
        };

        var income = new[]
        {
            (Id: Guid.Parse("20000000-0000-0000-0000-000000000001"), Name: "Salary",        Icon: "briefcase",     Color: "#22C55E"),
            (Id: Guid.Parse("20000000-0000-0000-0000-000000000002"), Name: "Freelance",     Icon: "laptop",        Color: "#3B82F6"),
            (Id: Guid.Parse("20000000-0000-0000-0000-000000000003"), Name: "Investments",   Icon: "trending-up",   Color: "#8B5CF6"),
            (Id: Guid.Parse("20000000-0000-0000-0000-000000000004"), Name: "Gifts",         Icon: "gift",          Color: "#EC4899"),
            (Id: Guid.Parse("20000000-0000-0000-0000-000000000005"), Name: "Other",         Icon: "ellipsis",      Color: "#9CA3AF"),
        };

        foreach (var c in expense)
            yield return new
            {
                Id = c.Id, UserId = (Guid?)null, Name = c.Name,
                Icon = c.Icon, Color = c.Color,
                Type = CategoryType.Expense,
                ParentCategoryId = (Guid?)null,
                CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null, DeletedAt = (DateTime?)null
            };

        foreach (var c in income)
            yield return new
            {
                Id = c.Id, UserId = (Guid?)null, Name = c.Name,
                Icon = c.Icon, Color = c.Color,
                Type = CategoryType.Income,
                ParentCategoryId = (Guid?)null,
                CreatedAt = DateTime.UtcNow, UpdatedAt = (DateTime?)null, DeletedAt = (DateTime?)null
            };
    }
}
