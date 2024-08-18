using FluentValidation;

namespace Catalog.UseCases.Categories.List;

public class ListCategoriesQueryValidator : AbstractValidator<ListCategoriesQuery>
{
    public ListCategoriesQueryValidator()
    {
        When(x => x.Skip.HasValue, () => RuleFor(x => x.Skip!.Value).GreaterThan(0));
        When(x => x.Take.HasValue, () => RuleFor(x => x.Take!.Value).GreaterThan(0));
    }
}