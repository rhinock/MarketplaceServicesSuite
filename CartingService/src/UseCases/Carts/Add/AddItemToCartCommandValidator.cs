using Carting.Infrastructure.Data.Config;
using Carting.UseCases.Helpers;
using Carting.UseCases.Invariants;
using FluentValidation;

namespace Carting.UseCases.Carts.Add;

public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(x => x.Id)
            .Must(ObjectIdHelper.IsValidObjectId)
            .WithMessage(ErrorMessages.InvalidIdFormat);

        RuleFor(x => x.Item).NotNull();

        When(x => x.Item is not null, () =>
        {
            RuleFor(x => x.Item.Id).GreaterThan(0);

            RuleFor(x => x.Item.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

            RuleFor(x => x.Item.Price).GreaterThan(0);
            RuleFor(x => x.Item.Quantity).GreaterThan(0);

            When(x => x.Item.Image is not null, () =>
            {
                RuleFor(x => x.Item.Image!.AltText).NotEmpty();
                RuleFor(x => x.Item.Image!.Url).NotEmpty();
            });
        });
    }
}