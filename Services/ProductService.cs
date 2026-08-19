using Microsoft.EntityFrameworkCore;
using ShopApp.Data;
using ShopApp.Models;

namespace ShopApp.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _context.Products
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _context.Products.AnyAsync(p => p.Id == id);
}
