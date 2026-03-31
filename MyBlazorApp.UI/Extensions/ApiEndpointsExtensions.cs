using MyBlazorApp.Application.Interfaces;

namespace MyBlazorApp.UI.Extensions;

public static class ApiEndpointsExtensions
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/products");

        group.MapGet("/", GetAllProducts);

        group.MapGet("/{id}", GetProductById);
    }

    private static async Task<IResult> GetAllProducts(IProductService productService)
    {
        var products = await productService.GetAllProductsAsync();
        return Results.Ok(products);
    }

    private static async Task<IResult> GetProductById(int id, IProductService productService)
    {
        var product = await productService.GetProductByIdAsync(id);
        return product != null ? Results.Ok(product) : Results.NotFound();
    }
}
