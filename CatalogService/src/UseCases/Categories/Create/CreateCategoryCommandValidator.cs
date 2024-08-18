using Catalog.Infrastructure.Data.Config;
using FluentValidation;

namespace Catalog.UseCases.Categories.Create;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

        When(x => x.ParentId.HasValue, () => RuleFor(x => x.ParentId!.Value).GreaterThan(0));
    }
}