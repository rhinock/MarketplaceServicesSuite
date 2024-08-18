using Catalog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCatalogServices(builder.Configuration);

var app = builder.Build();

await app.UseCatalogServices();

app.Run();
