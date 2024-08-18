using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using Catalog.Core.Categories;
using Catalog.Core.Categories.Specifications;
using Catalog.Core.Exceptions;
using Catalog.UseCases.Invariants;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.Categories.Get;

public class GetCategoryHandler(IReadRepository<Category> _repository, IMapper _mapper)
    : IQueryHandler<GetCategoryQuery, Result<CategoryResponse>>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var specification = new CategoryByIdSpecification(request.Id);

        var entity =
            await _repository.FirstOrDefaultAsync(specification, cancellationToken) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.CategoryNotFound, request.Id));

        var response = _mapper.Map<CategoryResponse>(entity);

        return response;
    }
}