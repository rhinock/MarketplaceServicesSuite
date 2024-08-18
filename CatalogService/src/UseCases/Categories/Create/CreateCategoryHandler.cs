using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using Catalog.Core.Categories;
using Catalog.Core.Exceptions;
using Catalog.Core.Interfaces;
using Catalog.UseCases.Invariants;
using Catalog.UseCases.Responses;
using Microsoft.AspNetCore.Http;

namespace Catalog.UseCases.Categories.Create;

public class CreateCategoryHandler(
    IRepository<Category> _repository,
    IMapper _mapper,
    ICategoryService _service)
    : ICommandHandler<CreateCategoryCommand, Result<CategoryResponse>>
{
    public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existingEntity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingEntity != null)
        {
            throw new EntityExistsException(string.Format(ErrorMessages.CategoryExists, request.Id));
        }

        if (_service.HasSelfReference(request.Id, request.ParentId))
        {
            throw new CircularReferenceException(
                string.Format(ErrorMessages.CategoryCircularReference, request.Id, request.ParentId));
        }

        if (request.ParentId.HasValue)
        {
            _ =
                await _repository.GetByIdAsync(request.ParentId.Value, cancellationToken) ??
                throw new EntityNotFoundException(string.Format(ErrorMessages.CategoryNotFound, request.ParentId.Value));
        }

        var newEntity = _mapper.Map<Category>(request);
        await _repository.AddAsync(newEntity, cancellationToken);
        var response = _mapper.Map<CategoryResponse>(newEntity);

        return response;
    }
}
