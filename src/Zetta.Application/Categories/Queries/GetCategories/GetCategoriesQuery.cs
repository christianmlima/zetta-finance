using MediatR;
using Zetta.Domain.Enums;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Categories.Queries.GetCategories;

public sealed record GetCategoriesQuery(Guid UserId) : IRequest<Result<List<CategoryResponse>>>;

public sealed record CategoryResponse(
    Guid Id,
    string Name,
    CategoryType Type,
    string Icon,
    string Color,
    Guid? ParentCategoryId,
    bool IsSystem);
