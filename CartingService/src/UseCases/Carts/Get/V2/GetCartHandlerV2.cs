using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using Carting.Core.Exceptions;
using Carting.Core.Interfaces;
using Carting.Responses;
using Carting.UseCases.Invariants;

namespace Carting.UseCases.Carts.Get.V2;

public class GetCartHandlerV2(ICartRepository _repository, IMapper _mapper)
    : IQueryHandler<GetCartQueryV2, Result<ICollection<ItemResponse>>>
{
    public async Task<Result<ICollection<ItemResponse>>> Handle(GetCartQueryV2 request, CancellationToken cancellationToken)
    {
        var entity =
            await _repository.GetByIdAsync(request.Id) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.CartNotFound, request.Id));

        var response = _mapper.Map<CartResponse>(entity);

        return Result.Success(response.Items);
    }
}