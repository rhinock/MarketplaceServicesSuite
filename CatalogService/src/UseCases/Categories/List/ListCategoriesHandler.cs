using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using Catalog.Core.Categories;
using Catalog.Core.Categories.Specifications;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.Categories.List;

public class ListCategoriesHandler(IReadRepository<Category> _repository, IMapper _mapper)
    : IQueryHandler<ListCategoriesQuery, Result<List<CategoryResponse>>>
{
    public async Task<Result<List<CategoryResponse>>> Handle(
        ListCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var specification = new CategoriesSpecification(request.Skip, request.Take);
        var entities = await _repository.ListAsync(specification, cancellationToken);
        var response = _mapper.Map<List<CategoryResponse>>(entities);

        return Result.Success(response);
    }
}