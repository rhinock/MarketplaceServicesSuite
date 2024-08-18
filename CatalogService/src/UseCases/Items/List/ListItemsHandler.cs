using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using Catalog.Core.Items;
using Catalog.Core.Items.Specifications;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.Items.List;

public class ListItemsHandler(IReadRepository<Item> _repository, IMapper _mapper)
    : IQueryHandler<ListItemsQuery, Result<List<ItemResponse>>>
{
    public async Task<Result<List<ItemResponse>>> Handle(ListItemsQuery request, CancellationToken cancellationToken)
    {
        var specification = new ItemsSpecification(request.CategoryId, request.Skip, request.Take);
        var entities = await _repository.ListAsync(specification, cancellationToken);
        var response = _mapper.Map<List<ItemResponse>>(entities);

        return Result.Success(response);
    }
}