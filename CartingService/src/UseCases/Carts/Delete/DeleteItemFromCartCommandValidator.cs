using Carting.UseCases.Helpers;
using Carting.UseCases.Invariants;
using FluentValidation;

namespace Carting.UseCases.Carts.Delete;

public class DeleteItemFromCartCommandValidator : AbstractValidator<DeleteItemFromCartCommand>
{
    public DeleteItemFromCartCommandValidator()
    {
        RuleFor(x => x.CartId)
            .Must(ObjectIdHelper.IsValidObjectId)
            .WithMessage(ErrorMessages.InvalidIdFormat);

        RuleFor(x => x.ItemId).GreaterThan(0);
    }
}