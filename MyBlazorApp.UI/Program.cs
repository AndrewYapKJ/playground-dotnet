using MyBlazorApp.UI.Components;
using MyBlazorApp.Application.Interfaces;
using MyBlazorApp.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5237") });
builder.Services.AddAntiforgery();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/api/products", async (IProductService productService) =>
{
    var products = await productService.GetAllProductsAsync();
    return Results.Ok(products);
});

app.MapGet("/api/products/{id:int}", async (int id, IProductService productService) =>
{
    var product = await productService.GetProductByIdAsync(id);
    return product != null ? Results.Ok(product) : Results.NotFound();
});

app.Run();
