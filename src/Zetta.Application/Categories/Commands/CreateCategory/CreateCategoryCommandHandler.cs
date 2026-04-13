using MediatR;
using Zetta.Domain.Aggregates.Categories;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Categories.Commands.CreateCategory;

internal sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CreateCategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = Category.Create(
            request.Name,
            request.Type,
            request.UserId,
            request.Icon,
            request.Color,
            request.ParentCategoryId);

        if (result.IsFailure)
            return Result.Failure<CreateCategoryResponse>(result.Error);

        _categoryRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var c = result.Value;
        return new CreateCategoryResponse(c.Id, c.Name, c.Type, c.Icon, c.Color);
    }
}
