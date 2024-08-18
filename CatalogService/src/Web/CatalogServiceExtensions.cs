using System.Reflection;
using Ardalis.SharedKernel;
using Catalog.Core.Categories;
using Catalog.UseCases.Categories.Create;
using FluentValidation.AspNetCore;
using MediatR;
using Catalog.UseCases;
using Catalog.Infrastructure;
using Catalog.UseCases.AutoMapper.Profiles;
using Catalog.Core.Interfaces;
using Catalog.Web.Middlewares;
using Microsoft.OpenApi.Models;

namespace Catalog.Web;

public static class CatalogServiceExtensions
{
    const string ServiceName = "CatalogService";
    const string ServiceVersion = "v1";

    public static IServiceCollection AddCatalogServices(
        this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(ServiceVersion, new OpenApiInfo { Title = ServiceName, Version = ServiceVersion });
        });

        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        services.AddUseCasesServices();

        ConfigureMediatR(services);

        services.AddInfrastructureServices(configurationManager);
        services.AddAutoMapper(typeof(Program), typeof(CategoryToCategoryResponseProfile));

        return services;
    }

    public static async Task<IApplicationBuilder> UseCatalogServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        await InitializeDatabase(app);

        app.MapControllers();
        app.UseHttpsRedirection();
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = string.Empty;
            c.SwaggerEndpoint($"/swagger/{ServiceVersion}/swagger.json", $"{ServiceName} {ServiceVersion}");
        });

        return app;
    }

    private static void ConfigureMediatR(IServiceCollection services)
    {
        var mediatRAssemblies = new[]
        {
            Assembly.GetAssembly(typeof(Category)), // Core
            Assembly.GetAssembly(typeof(CreateCategoryCommand)), // UseCases
        };

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }

    private static async Task InitializeDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();
        var databaseInitializationService = scope.ServiceProvider.GetRequiredService<IDatabaseInitializationService>();
        await databaseInitializationService.Initalize();
    }
}