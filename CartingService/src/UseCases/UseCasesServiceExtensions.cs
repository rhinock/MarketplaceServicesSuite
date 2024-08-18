using Carting.UseCases.Carts.Add;
using Carting.UseCases.Carts.Delete;
using Carting.UseCases.Carts.Get;
using Carting.UseCases.Carts.Get.V1;
using Carting.UseCases.Carts.Get.V2;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Carting.UseCases;

public static class UseCasesServiceExtensions
{
    public static IServiceCollection AddUseCasesServices(this IServiceCollection services)
    {
        services.AddTransient<IValidator<AddItemToCartCommand>, AddItemToCartCommandValidator>();
        services.AddTransient<IValidator<DeleteItemFromCartCommand>, DeleteItemFromCartCommandValidator>();
        services.AddTransient<IValidator<GetCartQueryV1>, GetCartQueryValidator>();
        services.AddTransient<IValidator<GetCartQueryV2>, GetCartQueryValidator>();

        return services;
    }
}