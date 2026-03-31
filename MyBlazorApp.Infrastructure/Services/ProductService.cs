using MyBlazorApp.Application.Interfaces;
using MyBlazorApp.Domain.Entities;
using MyBlazorApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MyBlazorApp.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync() 
        => await _context.Products.ToListAsync();

    public async Task<Product?> GetProductByIdAsync(int id) 
        => await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
}