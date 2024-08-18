using Catalog.UseCases.Items.Delete;
using FluentValidation;

namespace Catalog.Web.Items.Delete;

public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
{
    public DeleteItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}