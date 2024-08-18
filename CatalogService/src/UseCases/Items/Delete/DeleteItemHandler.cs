using Ardalis.Result;
using Ardalis.SharedKernel;
using Catalog.Core.Exceptions;
using Catalog.Core.Items;
using Catalog.UseCases.Invariants;
using Catalog.UseCases.Items.Delete;
using Common.RabbitMq;

namespace Catalog.UseCases.Categories.Delete;

public class DeleteItemHandler(IRepository<Item> _repository, RabbitMqClient _rabbitMqClient)
    : ICommandHandler<DeleteItemCommand, Result>
{
    public async Task<Result> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var entity =
            await _repository.GetByIdAsync(request.Id, cancellationToken) ??
            throw new EntityNotFoundException(string.Format(ErrorMessages.ItemNotFound, request.Id));

        await _repository.DeleteAsync(entity, cancellationToken);

        var message = new ItemDeleteMessage { Ids = [entity.Id] };
        _rabbitMqClient.Publish(message);

        return Result.Success();
    }
}
