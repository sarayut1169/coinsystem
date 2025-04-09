using System.Collections.Generic;
using coinsystem.Models;

namespace coinsystem.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<Product> AddProduct(int id, int amount);
        Task<Product> SubProduct(int id);
      
    }
}