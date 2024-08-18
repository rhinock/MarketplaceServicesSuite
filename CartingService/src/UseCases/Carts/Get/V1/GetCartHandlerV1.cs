using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using Carting.Core.Exceptions;
using Carting.Core.Interfaces;
using Carting.Responses;
using Carting.UseCases.Invariants;

namespace Carting.UseCases.Carts.Get.V1;

public class GetCartHandlerV1(ICartRepository _repository, IMapper _mapper)
    : IQueryHandler<GetCartQueryV1, Result<CartResponse>>
{
    public async Task<Result<CartResponse>> Handle(GetCartQueryV1 request, CancellationToken cancellationToken)
    {
        var entity =
            await _repository.GetByIdAsync(request.Id) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.CartNotFound, request.Id));

        var response = _mapper.Map<CartResponse>(entity);

        return response;
    }
}