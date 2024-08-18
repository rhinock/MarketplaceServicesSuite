using FluentValidation;

namespace Catalog.UseCases.Categories.Get;

public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
{
    public GetCategoryQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}