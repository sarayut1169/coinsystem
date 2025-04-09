using coinsystem.Services;
using coinsystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace coinsystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            Console.WriteLine(products.ToString());
            return Ok(products);
        }
        
        [HttpGet("getProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct(string name, double price,string picture_url)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name is required");
            }

            var product = new Product
            {
                Name = name,
                Price = price,
                Amount = 0,
                Picture = picture_url
            };

            var createdProduct = await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }
        
        [HttpPut("addProduct/{id}")]
        public async Task<IActionResult> UpdateProductAdd(int id, int amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Amount must be greater than zero");
            }

            var product = await _productService.AddProduct(id, amount);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        
        [HttpPut("subProduct/{id}")]
        public async Task<IActionResult> UpdateProductSub(int id)
        {
            var product = await _productService.SubProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpDelete("deleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(product);
            return NoContent();
        }
    }
}