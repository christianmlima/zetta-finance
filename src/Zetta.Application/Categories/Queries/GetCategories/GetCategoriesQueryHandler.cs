using MediatR;
using Zetta.Domain.Interfaces;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Categories.Queries.GetCategories;

internal sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<List<CategoryResponse>>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<List<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        // Returns system categories (UserId = null) + user's own categories
        var categories = await _categoryRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var response = categories
            .Select(c => new CategoryResponse(c.Id, c.Name, c.Type, c.Icon, c.Color, c.ParentCategoryId, c.IsSystem))
            .ToList();

        return response;
    }
}
