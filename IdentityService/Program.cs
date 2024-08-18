using System.Security.Claims;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var issuerUri = builder.Configuration["IdentityOptions:IssuerUri"];

var identityServerBuilder = builder.Services
    .AddIdentityServer(options =>
    {
        options.IssuerUri = issuerUri;
        // options.EmitStaticAudienceClaim = true;
    })
    .AddAspNetIdentity<IdentityUser>()
    .AddProfileService<ProfileService>()
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryClients(Config.GetClients())
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddDeveloperSigningCredential();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Service API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

if (!dbContext.Users.Any())
{
    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    var managerRoleName = "Manager";
    var buyerRoleName = "Buyer";

    var managerRole = new IdentityRole(managerRoleName);
    var buyerRole = new IdentityRole(buyerRoleName);

    await roleManager.CreateAsync(managerRole);
    await roleManager.CreateAsync(buyerRole);

    var user = new IdentityUser("manager");

    await userManager.CreateAsync(user, "Password$123");
    await userManager.AddToRoleAsync(user, managerRoleName);

    user = new IdentityUser("buyer");

    await userManager.CreateAsync(user, "Password$123");
    await userManager.AddToRoleAsync(user, buyerRoleName);

    var claimType = "Permission";
    var readClaimValue = "Read";

    await roleManager.AddClaimAsync(managerRole, new Claim(claimType, readClaimValue));
    await roleManager.AddClaimAsync(managerRole, new Claim(claimType, "Create"));
    await roleManager.AddClaimAsync(managerRole, new Claim(claimType, "Update"));
    await roleManager.AddClaimAsync(managerRole, new Claim(claimType, "Delete"));

    await roleManager.AddClaimAsync(buyerRole, new Claim(claimType, readClaimValue));
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service API V1");
});

app.UseRouting();
app.UseIdentityServer();
app.MapControllers();
app.Run();
