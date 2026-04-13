using MediatR;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(
    Guid CategoryId,
    Guid UserId,
    string Name,
    string Icon,
    string Color) : IRequest<Result>;
