using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using Catalog.Core.Exceptions;
using Catalog.Core.Items;
using Catalog.UseCases.Invariants;
using Catalog.UseCases.Responses;

namespace Catalog.UseCases.Items.Get;

public class GetItemHandler(IReadRepository<Item> _repository, IMapper _mapper)
    : IQueryHandler<GetItemQuery, Result<ItemResponse>>
{
    public async Task<Result<ItemResponse>> Handle(GetItemQuery request, CancellationToken cancellationToken)
    {
        var entity =
            await _repository.GetByIdAsync(request.Id, cancellationToken) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.ItemNotFound, request.Id));

        var response = _mapper.Map<ItemResponse>(entity);

        return response;
    }
}