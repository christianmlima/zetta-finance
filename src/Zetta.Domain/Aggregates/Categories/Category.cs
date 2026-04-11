using Zetta.Domain.Enums;
using Zetta.SharedKernel.Primitives;
using Zetta.SharedKernel.Results;

namespace Zetta.Domain.Aggregates.Categories;

public sealed class Category : AggregateRoot
{
    private Category(Guid id, Guid? userId, string name, string icon, string color, CategoryType type, Guid? parentCategoryId)
        : base(id)
    {
        UserId = userId;
        Name = name;
        Icon = icon;
        Color = color;
        Type = type;
        ParentCategoryId = parentCategoryId;
    }

    private Category() { }

    public Guid? UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Icon { get; private set; } = "tag";
    public string Color { get; private set; } = "#6366F1";
    public CategoryType Type { get; private set; }
    public Guid? ParentCategoryId { get; private set; }

    public bool IsSystem => UserId is null;

    public static Result<Category> Create(
        string name,
        CategoryType type,
        Guid? userId = null,
        string icon = "tag",
        string color = "#6366F1",
        Guid? parentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Category>(Error.Validation("Category name cannot be empty."));

        return new Category(Guid.NewGuid(), userId, name.Trim(), icon, color, type, parentCategoryId);
    }

    public void Update(string name, string icon, string color)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();

        Icon = icon;
        Color = color;
        SetUpdatedAt();
    }
}
