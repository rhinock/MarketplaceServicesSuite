using FluentValidation;

namespace Catalog.UseCases.Items.List;

public class ListItemsQueryValidator : AbstractValidator<ListItemsQuery>
{
    public ListItemsQueryValidator()
    {
        When(x => x.CategoryId.HasValue, () => RuleFor(x => x.CategoryId).GreaterThan(0));
        When(x => x.Skip.HasValue, () => RuleFor(x => x.Skip!.Value).GreaterThan(0));
        When(x => x.Take.HasValue, () => RuleFor(x => x.Take!.Value).GreaterThan(0));
    }
}