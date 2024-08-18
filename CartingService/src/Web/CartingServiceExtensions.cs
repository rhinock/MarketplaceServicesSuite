using FluentValidation.AspNetCore;
using Carting.UseCases;
using Carting.Infrastructure;
using Carting.UseCases.AutoMapper.Profiles;
using System.Reflection;
using Carting.Core.CartAggregate;
using Carting.UseCases.Carts.Add;
using MediatR;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Carting.Core.Interfaces;
using Carting.Web.Middlewares;
using Carting.Infrastructure.AutoMapper.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Carting.Web.Invariants;

namespace Carting.Web;

public static class CartingServiceExtensions
{
    const string ServiceName = "CartingService";
    const string ServiceVersion1 = "v1";
    const string ServiceVersion2 = "v2";

    public static IServiceCollection AddCartingServices(
        this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configurationManager["IdentityOptions:Authority"];
                options.Audience = "cartapi";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters.ValidTypes = ["at+jwt"];
            });

        services.AddAuthorization(options => 
        {
            options.AddPolicy(Policies.Buyer, cfg => 
            {
                cfg.RequireAuthenticatedUser();
                cfg.RequireClaim(Claims.ClaimType, Claims.BuyerClaimValues);
            });

            options.AddPolicy(Policies.Manager, cfg =>
            {
                cfg.RequireAuthenticatedUser();
                cfg.RequireClaim(Claims.ClaimType, Claims.ManagerClaimValues);
            });
        });

        services.AddControllers();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV"; ;
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(ServiceVersion1, new OpenApiInfo { Title = ServiceName, Version = ServiceVersion1 });
            c.SwaggerDoc(ServiceVersion2, new OpenApiInfo { Title = ServiceName, Version = ServiceVersion2 });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        services.AddUseCasesServices();
        ConfigureMediatR(services);

        services.AddInfrastructureServices(configurationManager);

        services.AddAutoMapper(
            typeof(Program),
            typeof(CartToCartResponseProfile),
            typeof(ItemUpdateMessageToItemProfile));

        return services;
    }

    public static async Task<IApplicationBuilder> UseCartingServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await InitializeDatabase(app);

        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        app.UseMiddleware<LogTokenMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = string.Empty;
            c.SwaggerEndpoint($"/swagger/{ServiceVersion1}/swagger.json", $"{ServiceName} {ServiceVersion1}");
            c.SwaggerEndpoint($"/swagger/{ServiceVersion2}/swagger.json", $"{ServiceName} {ServiceVersion2}");
        });

        return app;
    }

    private static void ConfigureMediatR(IServiceCollection services)
    {
        var mediatRAssemblies = new[]
        {
            Assembly.GetAssembly(typeof(Cart)), // Core
            Assembly.GetAssembly(typeof(AddItemToCartCommand)), // UseCases
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