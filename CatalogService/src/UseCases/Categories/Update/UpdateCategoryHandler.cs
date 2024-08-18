using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using Catalog.Core.Categories;
using Catalog.Core.Categories.Specifications;
using Catalog.Core.Exceptions;
using Catalog.Core.Interfaces;
using Catalog.UseCases.Invariants;
using Catalog.UseCases.Responses;
using Microsoft.AspNetCore.Http;

namespace Catalog.UseCases.Categories.Update;

public class UpdateCategoryHandler(
    IRepository<Category> _repository,
    IMapper _mapper,
    ICategoryService _service)
    : ICommandHandler<UpdateCategoryCommand, Result<CategoryResponse>>
{
    public async Task<Result<CategoryResponse>> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var specification = new CategoryByIdSpecification(request.Id);

        var existingEntity =
            await _repository.FirstOrDefaultAsync(specification, cancellationToken) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.CategoryNotFound, request.Id));

        if (request.ParentId.HasValue)
        {
            var _ =
                await _repository.GetByIdAsync(request.ParentId.Value, cancellationToken) ??
                throw new EntityNotFoundException(string.Format(ErrorMessages.CategoryNotFound, request.ParentId.Value));
        }

        if (await _service.HasCircularReferenceAsync(existingEntity, request.ParentId, cancellationToken))
        {
            throw new CircularReferenceException(
                string.Format(ErrorMessages.CategoryCircularReference, existingEntity.Id, request.ParentId));
        }

        _mapper.Map(request, existingEntity);
        await _repository.UpdateAsync(existingEntity, cancellationToken);
        var response = _mapper.Map<CategoryResponse>(existingEntity);

        return Result.Success(response);
    }
}