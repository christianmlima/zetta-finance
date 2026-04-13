using MediatR;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(Guid CategoryId, Guid UserId) : IRequest<Result>;
