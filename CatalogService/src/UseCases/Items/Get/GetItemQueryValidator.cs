using FluentValidation;

namespace Catalog.UseCases.Items.Get;

public class GetItemQueryValidator : AbstractValidator<GetItemQuery>
{
    public GetItemQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}