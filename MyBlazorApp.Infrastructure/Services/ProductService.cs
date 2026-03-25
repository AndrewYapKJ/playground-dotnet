using MyBlazorApp.Application.Interfaces;
using MyBlazorApp.Domain.Entities;

namespace MyBlazorApp.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 1500 },
        new Product { Id = 2, Name = "Phone", Price = 800 },
        new Product { Id = 3, Name = "Tablet", Price = 500 }
    };

    public Task<List<Product>> GetAllProductsAsync() => Task.FromResult(_products);

    public Task<Product?> GetProductByIdAsync(int id)
        => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
}