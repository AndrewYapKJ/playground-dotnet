using MyBlazorApp.Application.Interfaces;

namespace MyBlazorApp.UI.Extensions;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public static class ApiEndpointsExtensions
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/login", Login);
    }

    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/products");

        group.MapGet("/", GetAllProducts);

        group.MapGet("/{id}", GetProductById);
    }

    private static async Task<IResult> Login(LoginRequest request, IAuthService authService)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            return Results.BadRequest("Username and password are required");

        var token = await authService.AuthenticateAsync(request.Username, request.Password);

        if (token == null)
            return Results.Unauthorized();

        return Results.Ok(new { token });
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
