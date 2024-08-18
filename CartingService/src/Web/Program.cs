using Carting.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCartingServices(builder.Configuration);

var app = builder.Build();

await app.UseCartingServices();

app.Run();
