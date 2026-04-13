using MediatR;
using Zetta.Domain.Enums;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(
    Guid UserId,
    string Name,
    CategoryType Type,
    string Icon = "tag",
    string Color = "#6366F1",
    Guid? ParentCategoryId = null) : IRequest<Result<CreateCategoryResponse>>;

public sealed record CreateCategoryResponse(Guid Id, string Name, CategoryType Type, string Icon, string Color);
