using coinsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace coinsystem.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;


        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Product.ToListAsync(); 
        }
        
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
        }
        
        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        
        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
        
        public async Task DeleteProductAsync(Product product)
        {
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
        }
        
        public async Task<Product> AddProduct(int id, int amount)
        {
            _context.Entry(await GetProductByIdAsync(id)).State = EntityState.Modified;
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                product.Amount += amount;
                await _context.SaveChangesAsync();
            }
            return product;
        }
        
        public async Task<Product> SubProduct(int id)
        {
            _context.Entry(await GetProductByIdAsync(id)).State = EntityState.Modified;
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                product.Amount -= 1;
                await _context.SaveChangesAsync();
            }
            return product;
        }
    }
}