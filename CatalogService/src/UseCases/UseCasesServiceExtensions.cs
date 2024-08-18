using Catalog.UseCases.Categories.Create;
using Catalog.UseCases.Categories.Delete;
using Catalog.UseCases.Categories.Get;
using Catalog.UseCases.Categories.List;
using Catalog.UseCases.Items.Delete;
using Catalog.UseCases.Items.Create;
using Catalog.Web.Items.Delete;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Catalog.UseCases.Items.Get;
using Catalog.UseCases.Items.List;
using Catalog.UseCases.Categories.Update;
using Catalog.UseCases.Items.Update;

namespace Catalog.UseCases;

public static class UseCasesServiceExtensions
{
    public static IServiceCollection AddUseCasesServices(this IServiceCollection services)
    {
        services.AddTransient<IValidator<CreateCategoryCommand>, CreateCategoryCommandValidator>();
        services.AddTransient<IValidator<DeleteCategoryCommand>, DeleteCategoryCommandValidator>();
        services.AddTransient<IValidator<GetCategoryQuery>, GetCategoryQueryValidator>();
        services.AddTransient<IValidator<ListCategoriesQuery>, ListCategoriesQueryValidator>();
        services.AddTransient<IValidator<UpdateCategoryCommand>, UpdateCategoryCommandValidator>();

        services.AddTransient<IValidator<CreateItemCommand>, CreateItemCommandValidator>();
        services.AddTransient<IValidator<DeleteItemCommand>, DeleteItemCommandValidator>();
        services.AddTransient<IValidator<GetItemQuery>, GetItemQueryValidator>();
        services.AddTransient<IValidator<ListItemsQuery>, ListItemsQueryValidator>();
        services.AddTransient<IValidator<UpdateItemCommand>, UpdateItemCommandValidator>();

        return services;
    }
}