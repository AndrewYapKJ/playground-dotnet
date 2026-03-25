using MyBlazorApp.Domain.Entities;

namespace MyBlazorApp.Application.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
}