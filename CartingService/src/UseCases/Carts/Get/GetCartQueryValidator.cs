using Carting.Core.Interfaces;
using Carting.UseCases.Helpers;
using Carting.UseCases.Invariants;
using FluentValidation;

namespace Carting.UseCases.Carts.Get;

public class GetCartQueryValidator : AbstractValidator<IGetCartQuery>
{
    public GetCartQueryValidator()
    {
        RuleFor(x => x.Id)
            .Must(ObjectIdHelper.IsValidObjectId)
            .WithMessage(ErrorMessages.InvalidIdFormat);
    }
}